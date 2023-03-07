using System;
using System.Threading.Tasks;
using DiFY.Modules.Social.Application.Contracts;
using DiFY.Modules.Social.Application.Members;
using DiFY.Modules.Social.Application.Members.GetUserProfile;
using DiFY.WebAPI.Configuration.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiFY.WebAPI.Modules.Social.UserProfile;

[ApiController]
[Route("api/social/user_profile")]
public class UserProfileController : ControllerBase
{
    private readonly ISocialModule _socialModule;
    
    public UserProfileController(ISocialModule socialModule)
    {
        _socialModule = socialModule;
    }
    
    [HttpGet("{userId:guid}")]
    [HasPermission(UserProfilePermissions.CanGetUserProfile)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetUserProfileDto>> GetCall(Guid userId)
    {
        var userProfile = await _socialModule.ExecuteQueryAsync(new GetUserProfileQuery(userId));
        return userProfile;
    }
}
