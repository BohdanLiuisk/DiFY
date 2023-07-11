using Dify.Core.Application.Users.Commands.AuthenticateCommand;
using Dify.Core.Application.Users.Queries.GetAllUsers;
using Dify.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dify.Core.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/weatherForecast")]
public class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherForecastController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getUsers")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }
    
    [HttpPost("checkCreds")]
    public async Task<ActionResult<AuthenticationResult>> CheckCreds(string login, string password)
    {
        var authenticationResult = await _mediator.Send(new AuthenticateCommand(login, password));
        return Ok(authenticationResult);
    }
}
