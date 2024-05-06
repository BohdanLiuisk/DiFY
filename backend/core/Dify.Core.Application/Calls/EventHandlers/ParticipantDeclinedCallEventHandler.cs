using Dify.Core.Application.Common;
using Dify.Core.Domain.Enums;
using Dify.Core.Domain.Events;

namespace Dify.Core.Application.Calls.EventHandlers;

public class ParticipantDeclinedCallEventHandler : INotificationHandler<ParticipantDeclinedCall>
{
    private readonly IDifyContext _difyContext;

    public ParticipantDeclinedCallEventHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }
    
    public async Task Handle(ParticipantDeclinedCall notification, CancellationToken cancellationToken)
    {
        var participantsCount = await _difyContext.CallParticipants.CountAsync(
            p => p.CallId == notification.CallId && p.ParticipantId != notification.ParticipantId, 
            cancellationToken: cancellationToken);
        if (participantsCount == 1)
        {
            var callInitiator = await _difyContext.CallParticipants.FirstOrDefaultAsync(
                p => p.CallId == notification.CallId && p.ParticipantId == p.Call.CreatedById, 
                cancellationToken: cancellationToken);
            if (callInitiator != null)
            {
                callInitiator.Status = CallParticipantStatus.Declined;
            }
            var call = await _difyContext.Calls.FirstOrDefaultAsync(c => c.Id == notification.CallId,
                cancellationToken: cancellationToken);
            call.Active = false;
        }
    }
}
