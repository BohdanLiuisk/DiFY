namespace Dify.Common.Dto;

public class ParticipantForCallDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public bool IsOnline { get; set; }
    public string AvatarUrl { get; set; }
}
