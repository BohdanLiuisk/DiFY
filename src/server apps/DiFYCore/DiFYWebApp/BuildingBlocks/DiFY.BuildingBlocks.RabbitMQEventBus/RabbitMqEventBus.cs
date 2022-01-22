using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Infrastructure.EventBus;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Serilog;

namespace DiFY.BuildingBlocks.EventBus
{
    public class RabbitMqEventBus : IEventsBus, IDisposable
    {
        private const string BrokerName = "dify_event_bus";
        
        private readonly IRabbitMqPersistentConnection _persistentConnection;
        
        private readonly IEventBusSubscriptionsManager _subsManager;
        
        private readonly ILogger _logger;

        private readonly int _retryCount;
        
        private IModel _consumerChannel;
        
        private string _queueName;

        public RabbitMqEventBus(IRabbitMqPersistentConnection persistentConnection,
            IEventBusSubscriptionsManager subsManager, ILogger logger, string queueName = null, int retryCount = 5)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _queueName = queueName;
            _consumerChannel = CreateConsumerChannel();
            _retryCount = retryCount;
            _subsManager.OnEventRemoved += SubsManagerOnEventRemoved;
        }
        
        public void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.Warning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            var eventName = @event.GetType().Name;

            _logger.Information("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

            using var channel = _persistentConnection.CreateModel();
            
            _logger.Information("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);

            channel.ExchangeDeclare(exchange: BrokerName, type: "direct");
                                
            var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            });

            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                    
                properties.DeliveryMode = 2;

                _logger.Information("Publishing event to RabbitMQ: {EventId}", @event.Id);

                channel.BasicPublish(
                    exchange: BrokerName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        public void Subscribe<T, TH>(TH eventHandler) where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            
            SubscribeInternal(eventName);

            _logger.Information("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).FullName);

            _subsManager.AddSubscription<T, TH>(eventHandler);
            
            StartBasicConsume();
        }
        
        public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.Information("Unsubscribing from event {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>();
        }
            
        public void Dispose()
        {
            _consumerChannel?.Dispose();

            _subsManager.Clear();
        }
        
        private void SubscribeInternal(string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            
            if (containsKey) return;
            
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
 
            _consumerChannel.QueueBind(queue: _queueName,
                exchange: BrokerName,
                routingKey: eventName);
        }
        
        private void SubsManagerOnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using var channel = _persistentConnection.CreateModel();
            
            channel.QueueUnbind(
                queue: _queueName,
                exchange: BrokerName,
                routingKey: eventName);

            if (!_subsManager.IsEmpty) return;
            
            _queueName = string.Empty;
            
            _consumerChannel.Close();
        }
        
        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.Information("Creating RabbitMQ consumer channel...");

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BrokerName,
                type: "direct");

            channel.QueueDeclare(queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.Warning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                
                _consumerChannel = CreateConsumerChannel();
                
                StartBasicConsume();
            };

            return channel;
        }
        
        private void StartBasicConsume()
        {
            _logger.Information("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += ConsumerReceived;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.Error("StartBasicConsume can't call on _consumerChannel == null");
            }
        }
        
        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, "ERROR Processing message \"{Message}\"", message);
            }
            
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
        
        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.Information("Processing RabbitMQ event: {EventName}", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                
                foreach (var subscription in subscriptions)
                {
                    if (subscription.Handler == null) continue;
                    
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    
                    var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive= true});
                    
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                    await Task.Yield();
                    
                    await (Task)concreteType.GetMethod("Handle")?.Invoke(subscription.Handler, new object[] { integrationEvent });
                }
            }
            else
            {
                _logger.Warning($"No subscription for RabbitMQ event: {eventName}");
            }
        }
    }
}