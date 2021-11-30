using System.Collections.Generic;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.UserAccess.Domain.UserRegistrations;

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

        internal static User CreateFromUserRegistration(
            UserRegistrationId userRegistrationId,
            string login,
            string password,
            string email,
            string firstName,
            string lastName,
            string name)
        {
            return new User(userRegistrationId, login, password, email, firstName, lastName, name);
        }

        private User(
            UserRegistrationId userRegistrationId,
            string login,
            string password,
            string email,
            string firstName,
            string lastName,
            string name)
        {
            Id = new UserId(userRegistrationId.Value);
            _login = login;
            _password = password;
            _email = email;
            _firstName = firstName;
            _lastName = lastName;
            _name = name;

            _isActive = true;

            _roles = new List<UserRole>()
            {
                UserRole.Member
            };
        }
    }
}