using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.Calling.JoinCall;

public class JoinCallCommand : CommandBase
{
    public JoinCallCommand(Guid callId, string streamId, string peerId, string connectionId)
    {
        CallId = callId;
        StreamId = streamId;
        PeerId = peerId;
        ConnectionId = connectionId;
    }
    
    public string StreamId { get; }
    
    public string PeerId { get; }
    
    public string ConnectionId { get; }
    
    public Guid CallId { get; } 
}