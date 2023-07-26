namespace Dify.Common.Dto;

public class CallDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public bool Active { get; set; }
    
    public int? InitiatorId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public double Duration { get; set; }
    
    public int TotalParticipants { get; set; }
    
    public int ActiveParticipants { get; set; }
}
