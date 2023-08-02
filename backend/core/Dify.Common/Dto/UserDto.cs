namespace Dify.Common.Dto;

public record UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string AvatarUrl { get; set; }
    public bool Online { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool IsCurrentUser { get; set; }
};
