using Autofac;
using DiFY.BuildingBlocks.EventBus;
using DiFY.BuildingBlocks.Infrastructure.EventBus;
using RabbitMQ.Client;

namespace DiFY.Modules.Social.Infrastructure.Configuration.EventBus
{
    internal class EventsBusModule : Module
    {
        private readonly string _eventBusConnection;

        private readonly string _queueName;

        internal EventsBusModule(string eventBusConnection, string queueName)
        {
            _eventBusConnection = eventBusConnection;
            _queueName = queueName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryEventBusSubscriptionsManager>()
                .As<IEventBusSubscriptionsManager>()
                .SingleInstance();
            var connectionFactory = new ConnectionFactory()
            {
                HostName = _eventBusConnection,
                DispatchConsumersAsync = true
            };
            builder.RegisterType<DefaultRabbitMqPersistentConnection>()
                .As<IRabbitMqPersistentConnection>()
                .WithParameter("connectionFactory", connectionFactory)
                .SingleInstance();
            builder.RegisterType<RabbitMqEventBus>()
                .As<IEventsBus>()
                .WithParameter("queueName", _queueName)
                .SingleInstance();
        }
    }
}
