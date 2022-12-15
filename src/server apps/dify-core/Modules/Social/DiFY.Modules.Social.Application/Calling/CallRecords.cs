using System;
using System.Collections.Generic;

namespace DiFY.Modules.Social.Application.Calling;

public record GetAllCallsQueryResult
{
    public IEnumerable<GetAllCallsDto> Calls { get; set; } 
    
    public int TotalCount { get; set; }
};
    
public record GetAllCallsDto(Guid Id, string Name, bool Active, DateTime StartDate, DateTime EndDate, 
    int ActiveParticipants, int TotalParticipants);

public record GetCallQueryResult
{
    public GetCallDto Call { get; set; }
    
    public IEnumerable<CallParticipantDto> Participants { get; set; } 
}

public record GetCallDto(Guid Id, string Name, DateTime StartDate, int ActiveParticipants, int TotalParticipants);

public record CallParticipantDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public bool Active { get; set; }
    
    public DateTime JoinOn { get; set; }
};