using System.Collections.Generic;
using System.Threading.Tasks;
using DiFY.Modules.UserAccess.Application.Authorization.DTOs;
using DiFY.Modules.UserAccess.Application.Authorization.GetAuthenticatedUserPermissions;
using DiFY.Modules.UserAccess.Application.Contracts;
using DiFY.Modules.UserAccess.Application.Users.DTOs;
using DiFY.Modules.UserAccess.Application.Users.GetAuthenticatedUser;
using DiFY.WebAPI.Configuration.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiFY.WebAPI.Modules.UserAccess.Controllers
{
    [ApiController]
    [Route("api/userAccess/authUser")]
    public class AuthenticatedUserController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        public AuthenticatedUserController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        [NoPermissionRequired]
        [HttpGet("")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedUser()
        {
            var user = await _userAccessModule.ExecuteQueryAsync(new GetAuthenticatedUserQuery());
            return Ok(user);
        }

        [NoPermissionRequired]
        [HttpGet("permissions")]
        [ProducesResponseType(typeof(List<UserPermissionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedUserPermissions()
        {
            var permissions = await _userAccessModule.ExecuteQueryAsync(
                new GetAuthenticatedUserPermissionsQuery());
            return Ok(permissions);
        }
    }
}