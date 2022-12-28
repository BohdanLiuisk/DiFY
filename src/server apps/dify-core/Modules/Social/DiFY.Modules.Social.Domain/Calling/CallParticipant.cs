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

    private bool _active;

    private string _streamId;

    private string _peerId;

    private string _connectionId;
    
    private CallParticipant() { }

    private CallParticipant(CallId callId, MemberId participantId, DateTime joinDate, 
        string streamId, string peerId, string connectionId)
    {
        Id = new CallParticipantId(Guid.NewGuid());
        CallId = callId;
        ParticipantId = participantId;
        _joinDate = joinDate;
        _active = true;
        _streamId = streamId;
        _peerId = peerId;
        _connectionId = connectionId;
    }
    
    public static CallParticipant CreateNew(CallId callId, MemberId participantId, DateTime joinDate, string streamId,
        string peerId, string connectionId) => new(callId, participantId, joinDate, streamId, peerId, connectionId);

    internal bool IsActive() => _active;

    internal void MarkAsNotActive()
    {
        _active = false;
    }
    
    internal void MarkAsActive()
    {
        _active = true;
    }
    
    internal void SetConnectionData(string streamId, string peerId, string connectionId)
    {
        _streamId = streamId;
        _peerId = peerId;
        _connectionId = connectionId;
    }
    
    internal void ClearConnectionData()
    {
        _streamId = null;
        _peerId = null;
        _connectionId = null;
    }
}
