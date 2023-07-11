using Dify.Core.Application.Users.Commands.CreateNewUser;
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
    
    [AllowAnonymous]
    [HttpPost("createNew")]
    public async Task<ActionResult<int>> CreateUser(CreateNewUserCommand newUser)
    {
        var userId = await _mediator.Send(newUser);
        return Ok(userId);
    }
}
