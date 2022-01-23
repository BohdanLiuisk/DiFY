using System.Threading.Tasks;
using DiFY.BuildingBlocks.Infrastructure.EventBus;
using DiFY.Modules.Administration.Application.Members.CreateMember;
using DiFY.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace DiFY.Modules.Administration.Application.Members
{
    public class NewUserRegisteredIntegrationEventHandler : IIntegrationEventHandler<NewUserRegisteredIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public NewUserRegisteredIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task Handle(NewUserRegisteredIntegrationEvent @event)
        {
            await _mediator.Send(new CreateMemberCommand(
                @event.Id,
                @event.UserId,
                @event.Login,
                @event.Email,
                @event.FirstName,
                @event.LastName,
                @event.Name));
        }
    }
}