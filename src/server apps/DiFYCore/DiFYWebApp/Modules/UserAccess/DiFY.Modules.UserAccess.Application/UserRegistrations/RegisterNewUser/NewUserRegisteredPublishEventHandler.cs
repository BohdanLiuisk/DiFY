using System.Threading;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Infrastructure.EventBus;
using DiFY.Modules.UserAccess.Domain.UserRegistrations.Events;
using DiFY.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace DiFY.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser
{
    internal class NewUserRegisteredPublishEventHandler : INotificationHandler<NewUserRegisteredDomainEvent>
    {
        private readonly IEventsBus _eventsBus;

        public NewUserRegisteredPublishEventHandler(IEventsBus eventsBus)
        {
            _eventsBus = eventsBus;
        }

        public Task Handle(NewUserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            _eventsBus.Publish(new NewUserRegisteredIntegrationEvent(
                notification.Id,
                notification.OccurredOn,
                notification.UserRegistrationId.Value,
                notification.Login,
                notification.Email,
                notification.FirstName,
                notification.LastName,
                notification.Name));
            return Task.CompletedTask;
        }
    }
}