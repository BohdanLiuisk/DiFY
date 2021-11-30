using System;
using System.Threading;
using System.Threading.Tasks;
using DiFY.Modules.UserAccess.Application.Authentication;
using DiFY.Modules.UserAccess.Application.Configuration.Commands;
using DiFY.Modules.UserAccess.Domain.UserRegistrations;
using DiFY.Modules.UserAccess.Domain.UserRegistrations.Interfaces;

namespace DiFY.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser
{
    public class RegisterNewUserCommandHandler : ICommandHandler<RegisterNewUserCommand, Guid>
    {
        private readonly IUserRegistrationRepository _userRegistrationRepository;
        private readonly IUsersCounter _usersCounter;

        public RegisterNewUserCommandHandler(
            IUserRegistrationRepository userRegistrationRepository, IUsersCounter usersCounter)
        {
            _userRegistrationRepository = userRegistrationRepository;
            _usersCounter = usersCounter;
        }

        public async Task<Guid> Handle(RegisterNewUserCommand command, CancellationToken cancellationToken)
        {
            var password = PasswordHashManager.HashPassword(command.Password);

            var userRegistration = UserRegistration.RegisterNewUser(
                command.Login,
                password,
                command.Email,
                command.FirstName,
                command.LastName,
                _usersCounter,
                command.ConfirmLink);

            await _userRegistrationRepository.AddAsync(userRegistration);

            return userRegistration.Id.Value;
        }
    }
}