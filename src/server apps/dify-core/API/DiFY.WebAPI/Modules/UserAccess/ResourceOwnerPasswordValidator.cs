using System.Threading.Tasks;
using DiFY.Modules.UserAccess.Application.Authentication.Authenticate;
using DiFY.Modules.UserAccess.Application.Contracts;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace DiFY.WebAPI.Modules.UserAccess
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserAccessModule _userAccessModule;

        public ResourceOwnerPasswordValidator(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var authenticationResult = await _userAccessModule.ExecuteCommandAsync(
                new AuthenticateCommand(context.UserName, context.Password));
            if (!authenticationResult.IsAuthenticated)
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant, authenticationResult.AuthenticationError);
                return;
            }
            context.Result = new GrantValidationResult(
                subject: authenticationResult.User.Id.ToString(),
                authenticationMethod: "forms",
                claims: authenticationResult.User.Claims);
        }
    }
}