using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Users.Commands;
using Dify.Core.Application.Users.Queries;
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
    
    [HttpGet("getById/{id}")]
    public async Task<ActionResult<QueryResponse<UserDto>>> GetUser(int id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }
    
    [HttpGet("getUsers")]
    public async Task<ActionResult<QueryResponse<PaginatedList<UserDto>>>> GetUsers(int pageNumber, int pageSize)
    {
        var users = await _mediator.Send(
            new GetPaginatedUsersQuery(pageNumber, pageSize));
        return Ok(users);
    }
}
