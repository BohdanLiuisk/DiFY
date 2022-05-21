using System.Threading;
using System.Threading.Tasks;
using DiFY.Modules.Administration.Application.Members.CreateMember;
using DiFY.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace DiFY.Modules.Administration.Application.Members
{
    public class NewUserRegisteredIntegrationEventHandler : INotificationHandler<NewUserRegisteredIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public NewUserRegisteredIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(NewUserRegisteredIntegrationEvent @event, CancellationToken cancellationToken)
        {
             _mediator.Send(new CreateMemberCommand(
                @event.Id,
                @event.UserId,
                @event.Login,
                @event.Email,
                @event.FirstName,
                @event.LastName,
                @event.Name), cancellationToken);
            return Task.CompletedTask;
        }
    }
}