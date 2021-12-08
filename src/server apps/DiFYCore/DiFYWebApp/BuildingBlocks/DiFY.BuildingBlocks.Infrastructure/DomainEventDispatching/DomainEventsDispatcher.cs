using System;
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

        private readonly IDomainNotificationsMapper _domainNotificationsMapper;

        public DomainEventsDispatcher(
            IMediator mediator,
            ILifetimeScope scope,
            IDomainEventsAccessor domainEventsProvider,
            IDomainNotificationsMapper domainNotificationsMapper)
        {
            _mediator = mediator;
            _scope = scope;
            _domainEventsProvider = domainEventsProvider;
            _domainNotificationsMapper = domainNotificationsMapper;
        }
        
        public async Task DispatchEventsAsync()
        {
            var domainEvents = _domainEventsProvider.GetAllDomainEvents();

            var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();

            foreach (var domainEvent in domainEvents)
            {
                Type domainEventNotificationType = typeof(IDomainEventNotification<>);

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