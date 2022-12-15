using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.Calling.GetCall;

public class GetCallQuery : QueryBase<GetCallQueryResult>
{
    public GetCallQuery(Guid callId)
    {
        CallId = callId;
    } 
    
    public Guid CallId { get; set; }
}
