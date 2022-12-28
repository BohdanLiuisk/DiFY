using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.Calling.GetCurrentUserJoinedCall;

public class GetIsInitiatorAndJoinedCallQuery : QueryBase<bool>
{
    public GetIsInitiatorAndJoinedCallQuery(Guid callId)
    {
        CallId = callId;
    }
    
    public Guid CallId { get; set; }
}