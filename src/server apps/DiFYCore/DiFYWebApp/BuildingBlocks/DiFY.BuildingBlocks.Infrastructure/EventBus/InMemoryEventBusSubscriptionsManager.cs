using System;
using System.Collections.Generic;
using System.Linq;

namespace DiFY.BuildingBlocks.Infrastructure.EventBus
{
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<IntegrationEventSubscription>> _handlers;
        
        private readonly List<Type> _eventTypes;
        
        public event EventHandler<string> OnEventRemoved;

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<IntegrationEventSubscription>>();
            _eventTypes = new List<Type>();
        }
        
        public bool IsEmpty => _handlers is { Count: 0 };

        public void Clear() => _handlers.Clear();
        
        public string GetEventKey<T>() => typeof(T).Name;
        
        public void AddSubscription<T, TH>(TH eventHandler) where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            AddInternalSubscription(eventHandler,  eventName);
            
            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }

        public void RemoveSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            var eventName = GetEventKey<T>();
            RemoveInternalSubscription(eventName, handlerToRemove);
        }

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        public IEnumerable<IntegrationEventSubscription> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return GetHandlersForEvent(key);
        }
        
        public IEnumerable<IntegrationEventSubscription> GetHandlersForEvent(string eventName) => _handlers[eventName];
        
        private void AddInternalSubscription(IIntegrationEventHandler handler, string eventName)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<IntegrationEventSubscription>());
            }

            if (_handlers[eventName].Any(i => i.HandlerType.FullName == handler.GetType().FullName))
            {
                throw new ArgumentException(
                    $"Handler Type {handler.GetType().Name} already registered for '{eventName}'", nameof(handler));
            }
                
            _handlers[eventName].Add(IntegrationEventSubscription.Add(handler));
        }
        
        private void RemoveInternalSubscription(string eventName, IntegrationEventSubscription subsToRemove)
        {
            if (subsToRemove == null) return;

            _handlers[eventName].Remove(_handlers[eventName]
                .SingleOrDefault(subs => subs.HandlerType == subsToRemove.HandlerType));
            
            if (_handlers[eventName].Any()) return;
            
            _handlers.Remove(eventName);
            
            var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
            
            if (eventType != null)
            {
                _eventTypes.Remove(eventType);
            }
            
            RaiseOnEventRemoved(eventName);
        }
        
        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }
        
        private IntegrationEventSubscription FindSubscriptionToRemove<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            return FindInternalSubscriptionToRemove(eventName, typeof(TH));
        }

        private IntegrationEventSubscription FindInternalSubscriptionToRemove(string eventName, Type handlerType) =>
            !HasSubscriptionsForEvent(eventName) ? null : _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
    }
}