using System;
using System.Collections.Generic;

namespace DiFY.BuildingBlocks.Infrastructure.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        
        event EventHandler<string> OnEventRemoved;

        string GetEventKey<T>();

        void AddSubscription<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent;
        
        void RemoveSubscription<T>() where T : IntegrationEvent;
        
        bool HasSubscriptionsForEvent(string eventName);
        
        IEnumerable<IIntegrationEventHandler> GetHandlersForEvent(string eventName);

        Type GetEventType(string eventName);

        void Clear();
    }
}
