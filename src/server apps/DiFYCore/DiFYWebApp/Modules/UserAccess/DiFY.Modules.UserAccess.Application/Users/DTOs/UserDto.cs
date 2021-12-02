using System;

namespace DiFY.Modules.UserAccess.Application.Users.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Login { get; set; }
        
        public string Email { get; set; }
        
        public bool IsActive { get; set; }
    }
}