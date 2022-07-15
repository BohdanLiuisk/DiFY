using System;
using System.Collections.Generic;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.Participating;

namespace DiFY.Modules.Social.Domain.Calling;

public class Call : Entity, IAggregateRoot
{
    public CallId Id { get; private set; }

    private readonly List<Participant> _participants;

    public IReadOnlyList<Participant> Participants => _participants.AsReadOnly();

    private readonly Participant _caller;
    
    private readonly DateTime _startDate;

    private DateTime _endDate;

    private Duration _duration;

    private Call() { }

    private Call(DateTime startDate, Participant caller)
    {
        _startDate = startDate;
        _caller = caller;
        _participants = new List<Participant> { _caller };
    }

    public static Call CreateNewCall(DateTime startDate, Participant caller)
    {
        return new Call(startDate, caller);
    }

    public void AddParticipant(Participant participant)
    {
        _participants.Add(participant);
    }

    public void EndCall(DateTime endDate)
    {
        _endDate = endDate;
        var duration = (_endDate - _startDate).TotalMinutes;
        _duration = Duration.Of(duration);
    }
}