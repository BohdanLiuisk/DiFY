using System;
using System.Collections.Generic;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.Membership;

namespace DiFY.Modules.Social.Domain.Calling;

public class Call : Entity, IAggregateRoot
{
    public CallId Id { get; private set; }
    
    private readonly List<CallParticipant> _participants;
    
    public IReadOnlyList<CallParticipant> Participants => _participants.AsReadOnly();

    private readonly MemberId _initiatorId;
    
    private readonly DateTime _startDate;

    private DateTime? _endDate;

    private Duration _duration;

    private Call() { }

    private Call(DateTime startDate, MemberId initiatorId)
    {
        Id = new CallId(Guid.NewGuid());
        _startDate = startDate;
        _initiatorId = initiatorId;
        _participants = new List<CallParticipant>
        {
            CallParticipant.CreateNew(Id, _initiatorId, startDate)
        };
    }

    public static Call CreateNew(MemberId initiatorId, DateTime startDate)
    {
        return new Call(startDate, initiatorId);
    }

    public void Join(MemberId participantId, DateTime joinDate)
    {
        _participants.Add(CallParticipant.CreateNew(Id, participantId, joinDate));
    }
    
    public void End(DateTime endDate)
    {
        _endDate = endDate;
        var duration = (_endDate - _startDate)?.TotalMinutes;
        _duration = Duration.Of(duration);
    }
}