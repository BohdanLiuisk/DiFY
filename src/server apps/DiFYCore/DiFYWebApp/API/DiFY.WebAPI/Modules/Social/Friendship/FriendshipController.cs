using System;
using System.Threading.Tasks;
using DiFY.Modules.Social.Application.Contracts;
using DiFY.Modules.Social.Application.FriendshipRequest.ConfirmFriendshipRequest;
using DiFY.Modules.Social.Application.FriendshipRequests.CreateFriendshipRequest;
using DiFY.WebAPI.Configuration.Authorization;
using DiFY.WebAPI.Modules.Social.Friendship.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiFY.WebAPI.Modules.Social.Friendship;

[ApiController]
[Route("api/social/[controller]")]
public class FriendshipController : ControllerBase
{
    private readonly ISocialModule _socialModule;

    public FriendshipController(ISocialModule socialModule)
    {
        _socialModule = socialModule;
    }
    
    [HttpPost("sendRequest")]
    [HasPermission(FriendshipPermissions.CanSendFriendshipRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendFriendshipRequest(SendFriendshipRequest request) 
    {
        await _socialModule.ExecuteCommandAsync(
            new CreateFriendshipRequestCommand(request.RequesterId, request.AddresseeId, DateTime.UtcNow));
        return Ok();
    }
    
    [HttpPut("{friendshipRequestId:guid}/confirm")]
    [HasPermission(FriendshipPermissions.CanSendFriendshipRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmRequest(Guid friendshipRequestId)
    {
        await _socialModule.ExecuteCommandAsync(new ConfirmFriendshipRequestCommand(friendshipRequestId));
        return Ok();
    }
}
