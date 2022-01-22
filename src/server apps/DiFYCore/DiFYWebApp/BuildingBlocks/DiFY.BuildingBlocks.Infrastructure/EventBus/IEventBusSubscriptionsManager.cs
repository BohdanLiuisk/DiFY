using System;
using System.Collections.Generic;

namespace DiFY.BuildingBlocks.Infrastructure.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        
        event EventHandler<string> OnEventRemoved;
        
        void AddSubscription<T, TH>(TH eventHandler) where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
        
        void RemoveSubscription<T, TH>() where TH : IIntegrationEventHandler<T> where T : IntegrationEvent;
        
        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
        
        bool HasSubscriptionsForEvent(string eventName);
        
        Type GetEventTypeByName(string eventName);
        
        void Clear();
        
        IEnumerable<IntegrationEventSubscription> GetHandlersForEvent<T>() where T : IntegrationEvent;
        
        IEnumerable<IntegrationEventSubscription> GetHandlersForEvent(string eventName);
        
        string GetEventKey<T>();
    }
}