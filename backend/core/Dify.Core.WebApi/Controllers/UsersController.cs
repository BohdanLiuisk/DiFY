using Dify.Common;
using Dify.Common.Dto;
using Dify.Core.Application.Users.Commands.CreateNewUser;
using Dify.Core.Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dify.Core.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getById/{id}")]
    public async Task<ActionResult<QueryResponse<UserDto>>> GetUser(int id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }
    
    [AllowAnonymous]
    [HttpPost("createNew")]
    public async Task<ActionResult<int>> CreateUser(CreateNewUserCommand newUser)
    {
        var userId = await _mediator.Send(newUser);
        return Ok(userId);
    }
}
