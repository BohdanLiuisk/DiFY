using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.Membership;

namespace DiFY.Modules.Social.Domain.Calling.Events;

public class ParticipantLeftCallDomainEvent : DomainEventBase
{
    public MemberId ParticipantId { get; }

    public ParticipantLeftCallDomainEvent(MemberId participantId)
    {
        ParticipantId = participantId;
    }
}