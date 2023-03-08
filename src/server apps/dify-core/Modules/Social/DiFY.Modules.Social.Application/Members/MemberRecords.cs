using System;

namespace DiFY.Modules.Social.Application.Members;

public class GetUserProfileDto
{
    public Guid Id { get; set; }
    
    public string Login { get; set; }
    
    public string Email { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public string AvatarUrl { get; set; }
    
    public bool CurrentUser { get; set; }
    
    public bool Online { get; set; }
}
