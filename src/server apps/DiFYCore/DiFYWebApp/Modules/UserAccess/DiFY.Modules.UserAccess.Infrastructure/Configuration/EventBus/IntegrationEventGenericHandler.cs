using Autofac;
using DiFY.BuildingBlocks.Infrastructure.EventBus;
using MediatR;
using System.Threading.Tasks;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration.EventBus
{
    internal class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T> 
        where T : IntegrationEvent
    {
        public async Task Handle(T @event)
        {
            using var scope = UserAccessCompositionRoot.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            await mediator.Publish((INotification)@event);
        }
    }
}
