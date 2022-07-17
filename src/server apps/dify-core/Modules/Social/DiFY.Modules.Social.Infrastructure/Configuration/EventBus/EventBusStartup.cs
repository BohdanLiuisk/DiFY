using Autofac;
using DiFY.BuildingBlocks.Infrastructure.EventBus;
using DiFY.Modules.UserAccess.IntegrationEvents;
using Serilog;

namespace DiFY.Modules.Social.Infrastructure.Configuration.EventBus
{
    internal class EventBusStartup
    {
        public static void Initialize(ILogger logger)
        {
            SubscribeToIntegrationEvents(logger);
        }

        private static void SubscribeToIntegrationEvents(ILogger logger)
        {
            var eventBus = SocialCompositionRoot.BeginLifetimeScope().Resolve<IEventsBus>();
            SubscribeToIntegrationEvent<NewUserRegisteredIntegrationEvent>(eventBus, logger);
        }

        private static void SubscribeToIntegrationEvent<T>(IEventsBus eventBus, ILogger logger) where T : IntegrationEvent
        {
            logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
            eventBus.Subscribe(new IntegrationEventGenericHandler<T>());
        }
    }
}
