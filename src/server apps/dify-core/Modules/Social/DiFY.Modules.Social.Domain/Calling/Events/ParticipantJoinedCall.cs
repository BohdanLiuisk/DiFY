using System;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.Membership;

namespace DiFY.Modules.Social.Domain.Calling.Events;

public class ParticipantJoinedCall : DomainEventBase
{
    public MemberId ParticipantId { get; }
    
    public DateTime JoinOn { get; set; }
    
    public ParticipantJoinedCall(MemberId participantId, DateTime joinOn)
    {
        ParticipantId = participantId;
        JoinOn = joinOn;
    }
}
