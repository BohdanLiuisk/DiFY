using System;
using System.Collections.Generic;
using System.Linq;
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

    private string _name;

    private readonly MemberId _initiatorId;

    private MemberId _dropperId;

    private bool _active;
    
    private readonly DateTime _startDate;

    private DateTime? _endDate;

    private Duration _duration;

    private Call() { }

    private Call(string name, DateTime startDate, MemberId initiatorId)
    {
        Id = new CallId(Guid.NewGuid());
        _name = name;
        _startDate = startDate;
        _active = true;
        _initiatorId = initiatorId;
    }

    public static Call CreateNew(string name, MemberId initiatorId, DateTime startDate)
    {
        return new Call(name, startDate, initiatorId);
    }

    public void Join(MemberId participantId, string streamId, string peerId, string connectionId, DateTime joinDate)
    {
        CheckRule(new ExistingActiveParticipantCantJoinRule(_participants, participantId));
        CheckRule(new CantEndLeftOrJoinNotActiveCallRule(_active));
        var existingParticipant = _participants.FirstOrDefault(p => p.ParticipantId == participantId);
        if (existingParticipant != null && !existingParticipant.IsActive())
        {
            existingParticipant.MarkAsActive();
            existingParticipant.SetConnectionData(streamId, peerId, connectionId);
        }
        else
        {
            _participants.Add(CallParticipant.CreateNew(Id, participantId, joinDate, streamId, peerId, connectionId));
        }
        AddDomainEvent(new ParticipantJoinedCall(_initiatorId, joinDate));
    }

    public void Left(MemberId participantId)
    {
        CheckRule(new CantEndLeftOrJoinNotActiveCallRule(_active));
        CheckRule(new NotParticipantCantLeftOrEndCallRule(Participants, participantId));
        var existingParticipant = _participants.FirstOrDefault(p => p.ParticipantId == participantId);
        if (existingParticipant != null)
        {
            existingParticipant.MarkAsNotActive();
            existingParticipant.ClearConnectionData();
        }
        AddDomainEvent(new ParticipantLeftCallDomainEvent(participantId));
    }
    
    public CallSummary End(DateTime endDate, MemberId dropperId)
    {
        CheckRule(new CantEndLeftOrJoinNotActiveCallRule(_active));
        CheckRule(new NotParticipantCantLeftOrEndCallRule(Participants, dropperId));
        _dropperId = dropperId;
        _active = false;
        _endDate = endDate;
        var duration = (_endDate - _startDate)?.TotalMinutes;
        _duration = Duration.Of(duration);
        _participants.Where(p => p.IsActive()).Select(p => 
        { 
            p.MarkAsNotActive();
            return p;
        }).ToList();
        return CallSummary.CreateNew(Duration.Of(duration), Participants.Count);
    }
}