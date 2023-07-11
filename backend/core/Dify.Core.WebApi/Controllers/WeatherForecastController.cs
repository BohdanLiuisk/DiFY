using Dify.Core.Application.Common.Interfaces;
using Dify.Core.Application.Users.Commands.AuthenticateCommand;
using Dify.Core.Application.Users.Commands.CreateNewUser;
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
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IDifyContext _difyContext;
    private readonly IMediator _mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IDifyContext difyContext, IMediator mediator)
    {
        _logger = logger;
        _difyContext = difyContext;
        _mediator = mediator;
    }
    
    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost("createUser")]
    public async Task<ActionResult<int>> CreateUser(CreateNewUserCommand newUser)
    {
        var userId = await _mediator.Send(newUser);
        return Ok(userId);
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

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string Summary { get; set; }
}
