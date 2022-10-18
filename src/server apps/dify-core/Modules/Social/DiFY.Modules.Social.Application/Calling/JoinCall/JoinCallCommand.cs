using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.Calling.JoinCall;

public class JoinCallCommand : CommandBase
{
    public JoinCallCommand(Guid callId)
    {
        CallId = callId;
    }
    
    public Guid CallId { get; } 
}