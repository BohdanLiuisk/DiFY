using System.Threading;
using System.Threading.Tasks;
using DiFY.Modules.Social.Domain.Calling.Events;
using MediatR;

namespace DiFY.Modules.Social.Application.Calling.Events;

public class ParticipantJoinedCallEventHandler : INotificationHandler<ParticipantJoinedCall>
{
    
    
    public Task Handle(ParticipantJoinedCall notification, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}