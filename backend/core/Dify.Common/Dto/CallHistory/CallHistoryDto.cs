namespace Dify.Common.Dto.CallHistory;

public class CallHistoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public int? InitiatorId { get; set; }
    public DateTime StartDate { get; set; }
    public int Direction { get; set; }
    public int Status { get; set; }
    public IEnumerable<CallParticipantHistoryDto> Participants { get; set; }
}
