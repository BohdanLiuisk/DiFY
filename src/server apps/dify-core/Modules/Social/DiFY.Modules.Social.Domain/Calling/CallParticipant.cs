﻿using System;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.Membership;

namespace DiFY.Modules.Social.Domain.Calling;

public class CallParticipant : Entity
{
    public CallParticipantId Id { get; private set; }
    
    public CallId CallId { get; private set; }
    
    public MemberId ParticipantId { get; private set; }

    private readonly DateTime _joinDate;

    private bool _active;
    
    private CallParticipant() { }

    private CallParticipant(CallId callId, MemberId participantId, DateTime joinDate)
    {
        Id = new CallParticipantId(Guid.NewGuid());
        CallId = callId;
        ParticipantId = participantId;
        _joinDate = joinDate;
        _active = true;
    }

    public static CallParticipant CreateNew(CallId callId, MemberId participantId, DateTime joinDate)
    {
        return new CallParticipant(callId, participantId, joinDate);
    }

    internal bool IsActive() => _active;

    internal void MarkAsNotActive()
    {
        _active = false;
    }
    
    internal void MarkAsActive()
    {
        _active = true;
    }
}