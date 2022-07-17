using System;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.Membership;

namespace DiFY.Modules.Social.Domain.Calling;

public class CallParticipant : Entity
{
    public CallParticipantId Id { get; private set; }
    
    public CallId CallId { get; private set; }
    
    public MemberId ParticipantId { get; private set; }

    private readonly DateTime _joinDate;
    
    private CallParticipant() { }

    private CallParticipant(CallId callId, MemberId participantId, DateTime joinDate)
    {
        Id = new CallParticipantId(Guid.NewGuid());
        CallId = callId;
        ParticipantId = participantId;
        _joinDate = joinDate;
    }

    public static CallParticipant CreateNew(CallId callId, MemberId participantId, DateTime joinDate)
    {
        return new CallParticipant(callId, participantId, joinDate);
    }
}