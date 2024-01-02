using Dify.Common.Dto.Friends;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Friends.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dify.Core.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/friends")]
public class FriendsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public FriendsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("findFriends")]
    public async Task<ActionResult<QueryResponse<PaginatedList<FoundFriendDto>>>>
        FindFriends(int pageNumber, int pageSize, string searchValue)
    {
        var friends = await _mediator.Send(new FindPaginatedFriendsQuery(pageNumber, pageSize, searchValue));
        return Ok(friends);
    }
}
