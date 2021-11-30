using System.Threading.Tasks;
using DiFY.Modules.UserAccess.Application.Contracts;
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
            var authentificationResult = await _userAccessModule.ExecuteCommandAsync()
        }
    }
}