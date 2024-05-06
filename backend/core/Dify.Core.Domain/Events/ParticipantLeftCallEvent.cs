using Dify.Core.Domain.Common;
using Dify.Core.Domain.Entities;

namespace Dify.Core.Domain.Events;

public class ParticipantLeftCallEvent : BaseEvent
{
    public CallParticipant Participant { get; set; }
    
    public ParticipantLeftCallEvent(CallParticipant participant)
    {
        Participant = participant;
    }
}
