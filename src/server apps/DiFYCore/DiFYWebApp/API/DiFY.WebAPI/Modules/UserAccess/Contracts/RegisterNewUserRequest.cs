﻿namespace DiFY.WebAPI.Modules.UserAccess.Contracts
{
    public class RegisterNewUserRequest
    {
        public string Login { get; set; }
        
        public string Password { get; set; }
        
        public string Email { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string ConfirmLink { get; set; }
    }
}