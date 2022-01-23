using Autofac;
using DiFY.BuildingBlocks.Infrastructure.EventBus;
using DiFY.Modules.Administration.Application.Members;
using DiFY.Modules.UserAccess.IntegrationEvents;
using MediatR;
using Serilog;

namespace DiFY.Modules.Administration.Infrastructure.Configuration.EventBus
{
    public class EventsBusStartup
    {
        public static void Initialize(ILogger logger)
        {
            SubscribeToIntegrationEvents(logger);
        }

        private static void SubscribeToIntegrationEvents(ILogger logger)
        {
            var eventBus = AdministrationCompositionRoot.BeginLifetimeScope().Resolve<IEventsBus>();
            var mediator = AdministrationCompositionRoot.BeginLifetimeScope().Resolve<IMediator>();
            
            eventBus.Subscribe<NewUserRegisteredIntegrationEvent, NewUserRegisteredIntegrationEventHandler>(new NewUserRegisteredIntegrationEventHandler(mediator));
        }
    }
}