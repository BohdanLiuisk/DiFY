using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application;
using DiFY.Modules.UserAccess.Application.Authorization.DTOs;
using DiFY.Modules.UserAccess.Application.Authorization.GetUserPermissions;
using DiFY.Modules.UserAccess.Application.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace DiFY.WebAPI.Configuration.Authorization
{
    internal class HasPermissionAuthorizationHandler 
        : AttributeAuthorizationHandler<HasPermissionAuthorizationRequirement, HasPermissionAttribute>
    {
        private readonly IExecutionContextAccessor _executionContextAccessor;

        private readonly IUserAccessModule _userAccessModule;

        public HasPermissionAuthorizationHandler(
            IExecutionContextAccessor executionContextAccessor,
            IUserAccessModule userAccessModule)
        {
            _executionContextAccessor = executionContextAccessor;
            _userAccessModule = userAccessModule;
        }
        
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            HasPermissionAuthorizationRequirement requirement, HasPermissionAttribute attribute)
        {
            var permissions = await _userAccessModule
                .ExecuteQueryAsync(new GetUserPermissionsQuery(_executionContextAccessor.UserId));
            if (!await AuthorizeAsync(attribute.Name, permissions))
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
        }

        private Task<bool> AuthorizeAsync(string permission, IEnumerable<UserPermissionDto> permissions)
        {
            return Task.FromResult(permissions.Any(x => x.Code == permission));
        }
    }
}