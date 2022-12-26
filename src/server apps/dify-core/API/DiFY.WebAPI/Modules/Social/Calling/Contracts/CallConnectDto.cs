using System;
using DiFY.Modules.Social.Application.Calling;

namespace DiFY.WebAPI.Modules.Social.Calling.Contracts;

public class CallConnectDto
{
    public Guid CallId { get; set; }
    
    public Guid UserId { get; set; }
    
    public string PeerId { get; set; }
    
    public string StreamId { get; set; }
    
    public CallParticipantDto Participant { get; set; }
}
