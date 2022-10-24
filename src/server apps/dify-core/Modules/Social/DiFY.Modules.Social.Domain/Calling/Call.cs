using System;
using System.Collections.Generic;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.Calling.Events;
using DiFY.Modules.Social.Domain.Calling.Rules;
using DiFY.Modules.Social.Domain.Membership;

namespace DiFY.Modules.Social.Domain.Calling;

public class Call : Entity, IAggregateRoot
{
    public CallId Id { get; private set; }
    
    private readonly List<CallParticipant> _participants;
    
    public IReadOnlyList<CallParticipant> Participants => _participants.AsReadOnly();

    private readonly MemberId _initiatorId;

    private MemberId _dropperId;

    private bool _active;
    
    private readonly DateTime _startDate;

    private DateTime? _endDate;

    private Duration _duration;

    private Call() { }

    private Call(DateTime startDate, MemberId initiatorId)
    {
        Id = new CallId(Guid.NewGuid());
        _startDate = startDate;
        _active = true;
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
        CheckRule(new CantEndLeftOrJoinNotActiveCallRule(_active));
        _participants.Add(CallParticipant.CreateNew(Id, participantId, joinDate));
    }

    public void Left(MemberId memberId)
    {
        CheckRule(new CantEndLeftOrJoinNotActiveCallRule(_active));
        CheckRule(new ParticipantExistsInCallRule(Participants, memberId));
        AddDomainEvent(new ParticipantLeftCallDomainEvent(memberId));
    }

    public CallSummary End(DateTime endDate, MemberId dropperId)
    {
        CheckRule(new CantEndLeftOrJoinNotActiveCallRule(_active));
        _dropperId = dropperId;
        _active = false;
        _endDate = endDate;
        var duration = (_endDate - _startDate)?.TotalMinutes;
        _duration = Duration.Of(duration);
        return CallSummary.CreateNew(Duration.Of(duration), Participants.Count);
    }
}