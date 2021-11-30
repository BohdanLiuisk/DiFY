using System.Collections.Generic;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.UserAccess.Domain.Users
{
    public class User : Entity, IAggregateRoot
    {
        public UserId Id { get; private set; }

        private string _login;

        private string _password;

        private string _email;

        private bool _isActive;

        private string _firstName;

        private string _lastName;

        private string _name;

        private List<UserRole> _roles;

        private User()
        {
            
        }
        
        internal static  User CreateFromUserRegistration()
    }
}