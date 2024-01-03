namespace Dify.Common.Dto;

public record CallParticipantDto
{
    public int Id { get; set; }
    public int ParticipantId { get; set; }
    public string Name { get; set; }
    public string StreamId { get; set; }
    public string PeerId { get; set; }
    public string ConnectionId { get; set; }
    public bool Active { get; set; }
    public DateTime JoinedAt { get; set; }
}
