using System;

namespace DiFY.Modules.Social.Application.Calling.GetAllCalls;

public class CallDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public bool Active { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public int ActiveParticipants { get; set; }
    
    public int TotalParticipants { get; set; }
}
