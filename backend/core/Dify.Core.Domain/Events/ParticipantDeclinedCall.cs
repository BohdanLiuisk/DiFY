using Dify.Core.Domain.Common;

namespace Dify.Core.Domain.Events;

public class ParticipantDeclinedCall : BaseEvent
{
    public Guid CallId { get; set; }
    
    public int ParticipantId { get; set; }

    public ParticipantDeclinedCall(Guid callId, int participantId)
    {
        CallId = callId;
        ParticipantId = participantId;
    }
}
