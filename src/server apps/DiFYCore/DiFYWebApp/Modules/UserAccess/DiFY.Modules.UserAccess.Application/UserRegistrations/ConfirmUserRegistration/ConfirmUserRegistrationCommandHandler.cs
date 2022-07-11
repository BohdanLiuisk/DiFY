using System.Threading;
using System.Threading.Tasks;
using DiFY.Modules.UserAccess.Application.Configuration.Commands;
using DiFY.Modules.UserAccess.Domain.UserRegistrations;
using DiFY.Modules.UserAccess.Domain.UserRegistrations.Interfaces;
using MediatR;

namespace DiFY.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration
{
    internal class ConfirmUserRegistrationCommandHandler : ICommandHandler<ConfirmUserRegistrationCommand>
    {
        private readonly IUserRegistrationRepository _userRegistrationRepository;

        public ConfirmUserRegistrationCommandHandler(IUserRegistrationRepository userRegistrationRepository)
        {
            _userRegistrationRepository = userRegistrationRepository;
        }

        public async Task<Unit> Handle(ConfirmUserRegistrationCommand command, CancellationToken cancellationToken)
        {
            var userRegistration = await _userRegistrationRepository
                .GetByIdAsync(new UserRegistrationId(command.UserRegistrationId));
            userRegistration.Confirm();
            return Unit.Value;
        }
    }
}