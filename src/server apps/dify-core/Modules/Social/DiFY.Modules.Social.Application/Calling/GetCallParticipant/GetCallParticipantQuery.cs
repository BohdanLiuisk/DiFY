using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.Calling.GetCallParticipant;

public class GetCallParticipantQuery : QueryBase<CallParticipantDto>
{
    public GetCallParticipantQuery(Guid participantId)
    {
        ParticipantId = participantId;
    } 
    
    public Guid ParticipantId { get; set; }
}
