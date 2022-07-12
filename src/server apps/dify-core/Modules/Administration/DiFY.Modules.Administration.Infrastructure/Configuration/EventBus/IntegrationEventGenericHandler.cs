using Autofac;
using DiFY.BuildingBlocks.Infrastructure.EventBus;
using MediatR;
using System.Threading.Tasks;

namespace DiFY.Modules.Administration.Infrastructure.Configuration.EventBus
{
    internal class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T>
        where T : IntegrationEvent
    {
        public async Task Handle(T @event)
        {
            var mediator = AdministrationCompositionRoot.BeginLifetimeScope().Resolve<IMediator>();
            await mediator.Publish(@event as INotification);
        }
    }
}
