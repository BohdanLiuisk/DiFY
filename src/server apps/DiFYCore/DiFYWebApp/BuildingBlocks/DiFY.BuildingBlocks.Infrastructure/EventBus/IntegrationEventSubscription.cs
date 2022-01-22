using System;

namespace DiFY.BuildingBlocks.Infrastructure.EventBus
{
    public class IntegrationEventSubscription
    {
        public Type HandlerType { get; }

        public IIntegrationEventHandler Handler { get;  }

        private IntegrationEventSubscription(IIntegrationEventHandler handler)
        {
            Handler = handler;
            HandlerType = handler.GetType();
        }

        public static IntegrationEventSubscription Add(IIntegrationEventHandler handler) =>
            new(handler);
    }
}