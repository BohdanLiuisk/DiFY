using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.Calling.LeftCall;

public class LeftCallCommand : CommandBase
{
    public LeftCallCommand(Guid callId)
    {
        CallId = callId;
    }
    
    public Guid CallId { get; }
}