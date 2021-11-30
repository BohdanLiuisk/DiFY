using System;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.UserAccess.Domain.UserRegistrations.Interfaces;
using DiFY.Modules.UserAccess.Domain.UserRegistrations.Rules;
using DiFY.Modules.UserAccess.Domain.Users;

namespace DiFY.Modules.UserAccess.Domain.UserRegistrations
{
    public class UserRegistration : Entity, IAggregateRoot
    {
        public UserRegistrationId Id { get; private set; }

        private string _login;

        private string _password;

        private string _email;

        private string _firstName;

        private string _lastName;

        private string _name;

        private DateTime _registerDate;

        private UserRegistrationStatus _status;

        private DateTime? _confirmedDate;

        private UserRegistration()
        {
            
        }
        
        private UserRegistration(
            string login,
            string password,
            string email,
            string firstName,
            string lastName,
            IUsersCounter usersCounter,
            string confirmLink)
        {
            CheckRule(new UserLoginMustBeUniqueRule(usersCounter, login));

            Id = new UserRegistrationId(Guid.NewGuid());
            _login = login;
            _password = password;
            _email = email;
            _firstName = firstName;
            _lastName = _lastName;
            _name = $"{firstName} {lastName}";
            _registerDate = DateTime.UtcNow;
            _status = UserRegistrationStatus.WaitingForConfirmation;
            
        }

        public static UserRegistration RegisterNewUser(
            string login,
            string password,
            string email,
            string firstName,
            string lastName,
            IUsersCounter usersCounter,
            string confirmLink)
        {
            return new UserRegistration(login, password, email, firstName, lastName, usersCounter, confirmLink);
        }
        
        public User CreateUser()
        {
            CheckRule(new UserCannotBeCreatedWhenRegistrationIsNotConfirmedRule(_status));
            
            
        }
    }
}