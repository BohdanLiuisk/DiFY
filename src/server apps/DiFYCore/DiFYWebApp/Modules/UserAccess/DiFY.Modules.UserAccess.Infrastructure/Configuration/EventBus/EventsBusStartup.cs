using Autofac;
using DiFY.BuildingBlocks.Infrastructure.EventBus;
using Serilog;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration.EventBus
{
    public class EventsBusStartup
    {
        public static void Initialize(ILogger logger)
        {
            SubscribeToIntegrationEvents(logger);
        }

        private static void SubscribeToIntegrationEvents(ILogger logger)
        {
            var eventBus = UserAccessCompositionRoot.BeginLifetimeScope().Resolve<IEventsBus>();
        }
    }
}