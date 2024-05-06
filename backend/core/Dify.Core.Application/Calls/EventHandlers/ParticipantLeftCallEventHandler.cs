using Dify.Core.Application.Common;
using Dify.Core.Domain.Enums;
using Dify.Core.Domain.Events;

namespace Dify.Core.Application.Calls.EventHandlers;

public class ParticipantLeftCallEventHandler : INotificationHandler<ParticipantLeftCallEvent>
{
    private readonly IDifyContext _difyContext;

    public ParticipantLeftCallEventHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }
    
    public async Task Handle(ParticipantLeftCallEvent notification, CancellationToken cancellationToken)
    {
        var callId = notification.Participant.CallId;
        var call = await _difyContext.Calls
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == callId, cancellationToken);
        var participantLeftCount = call.Participants.Count(
            p => p.Status is CallParticipantStatus.Active 
            or CallParticipantStatus.NotAnswered 
            or CallParticipantStatus.Declined);
        if (participantLeftCount == 0)
        {
            call.Active = false;
        }
    }
}
