using System.Threading.Tasks;
using Autofac;
using DiFY.BuildingBlocks.Infrastructure.Interfaces;
using MediatR;

namespace DiFY.BuildingBlocks.Infrastructure.DomainEventDispatching
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;

        private readonly IDomainEventsAccessor _domainEventsProvider;

        public DomainEventsDispatcher(
            IMediator mediator,
            IDomainEventsAccessor domainEventsProvider)
        {
            _mediator = mediator;
            _domainEventsProvider = domainEventsProvider;
        }
        
        public async Task DispatchEventsAsync()
        {
            var domainEvents = _domainEventsProvider.GetAllDomainEvents();
            _domainEventsProvider.ClearAllDomainEvents();
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }
    }
}