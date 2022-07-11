using System;
using System.Collections.Generic;
using System.Linq;

namespace DiFY.BuildingBlocks.Infrastructure.EventBus
{
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly IDictionary<string, List<IIntegrationEventHandler>> _handlers;

        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<IIntegrationEventHandler>>();
            _eventTypes = new List<Type>();
        }
        
        public bool IsEmpty => _handlers is { Count: 0 };

        public void Clear() => _handlers.Clear();
        
        public string GetEventKey<T>() => typeof(T).Name;

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public IEnumerable<IIntegrationEventHandler> GetHandlersForEvent(string eventName) => _handlers[eventName];

        public Type GetEventType(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        public void AddSubscription<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
        {
            var eventName = GetEventKey<T>();
            if (eventName == null) return;
            if (_handlers.ContainsKey(eventName))
            {
                var handlers = _handlers[eventName];
                handlers.Add(handler);
            }
            else
            {
                _handlers.Add(eventName, new List<IIntegrationEventHandler> { handler });
            }
            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }

        public void RemoveSubscription<T>() where T : IntegrationEvent
        {
            var eventName = GetEventKey<IntegrationEvent>();
            if (eventName == null || !_handlers.ContainsKey(eventName)) return;
            _handlers.Remove(eventName);
            RaiseOnEventRemoved(eventName);
            var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
            if (eventType != null)
            {
                _eventTypes.Remove(eventType);
            }
        }
        
        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }
    }
}
