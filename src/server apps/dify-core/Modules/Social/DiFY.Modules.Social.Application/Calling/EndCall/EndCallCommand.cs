using System;
using DiFY.Modules.Social.Application.Contracts;
using DiFY.Modules.Social.Domain.Calling;

namespace DiFY.Modules.Social.Application.Calling.EndCall;

public class EndCallCommand  : CommandBase<CallSummary>
{
    public EndCallCommand(Guid callId, DateTime endDate)
    {
        CallId = callId;
        EndDate = endDate;
    }
    
    public Guid CallId { get; }
    
    public DateTime EndDate { get; }
}