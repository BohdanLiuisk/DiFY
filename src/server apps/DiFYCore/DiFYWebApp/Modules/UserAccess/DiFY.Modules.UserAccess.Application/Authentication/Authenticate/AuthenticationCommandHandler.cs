using System.Threading;
using System.Threading.Tasks;
using DiFY.Modules.UserAccess.Application.Configuration.Commands;

namespace DiFY.Modules.UserAccess.Application.Authentication.Authenticate
{
    public class AuthenticationCommandHandler : ICommandHandler<AuthenticateCommand, AuthenticationResult>
    {
        public Task<AuthenticationResult> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}