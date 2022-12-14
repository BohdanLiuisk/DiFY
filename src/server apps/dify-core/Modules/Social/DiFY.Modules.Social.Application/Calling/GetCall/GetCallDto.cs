using System;
using System.Collections.Generic;

namespace DiFY.Modules.Social.Application.Calling.GetCall;

public class GetCallDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public int ActiveParticipants { get; set; }
    
    public int TotalParticipants { get; set; }
    
    public IEnumerable<CallParticipantDto> Participants { get; set; }
}

public class CallParticipantDto
{
    public Guid ParticipantId { get; set; }
    
    public string ParticipantName { get; set; }
    
    public bool Active { get; set; }
    
    public DateTime JoinOn { get; set; }
}
