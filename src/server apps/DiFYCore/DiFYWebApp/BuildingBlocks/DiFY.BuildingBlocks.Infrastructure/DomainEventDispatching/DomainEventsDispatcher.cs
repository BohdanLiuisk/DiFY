using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using DiFY.BuildingBlocks.Application.Events;
using DiFY.BuildingBlocks.Domain;
using DiFY.BuildingBlocks.Infrastructure.Interfaces;
using MediatR;

namespace DiFY.BuildingBlocks.Infrastructure.DomainEventDispatching
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;

        private readonly ILifetimeScope _scope;

        private readonly IDomainEventsAccessor _domainEventsProvider;

        public DomainEventsDispatcher(
            IMediator mediator,
            ILifetimeScope scope,
            IDomainEventsAccessor domainEventsProvider)
        {
            _mediator = mediator;
            _scope = scope;
            _domainEventsProvider = domainEventsProvider;
        }
        
        public async Task DispatchEventsAsync()
        {
            var domainEvents = _domainEventsProvider.GetAllDomainEvents();

            var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();

            foreach (var domainEvent in domainEvents)
            {
                var domainEventNotificationType = typeof(IDomainEventNotification<>);

                var domainNotificationWithGenericType =
                    domainEventNotificationType.MakeGenericType(domainEvent.GetType());

                var domainNotification = _scope.ResolveOptional(domainNotificationWithGenericType, new List<Parameter>
                {
                    new NamedParameter("domainEvent", domainEvent),
                    new NamedParameter("id", domainEvent.Id)
                });

                if (domainNotification != null)
                {
                    domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);
                }
            }
            
            _domainEventsProvider.ClearAllDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }
    }
}