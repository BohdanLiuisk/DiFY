using System;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.UserAccess.Domain.UserRegistrations.Events;
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

        private UserRegistration() { }

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
            _lastName = lastName;
            _name = $"{firstName} {lastName}";
            _registerDate = DateTime.UtcNow;
            _status = UserRegistrationStatus.WaitingForConfirmation;
            AddDomainEvent(new NewUserRegisteredDomainEvent(
                Id, _login, _email, _firstName, _lastName, _name, _registerDate, confirmLink));
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
            return User.CreateFromUserRegistration(Id, _login, _password, _email, _firstName, _lastName, _name);
        }

        public void Confirm()
        {
            _status = UserRegistrationStatus.Confirmed;
            _confirmedDate = DateTime.UtcNow;
            AddDomainEvent(new UserRegistrationConfirmedDomainEvent(Id));
        }

        public void Expire()
        {
            _status = UserRegistrationStatus.Expired;
        }
    }
}