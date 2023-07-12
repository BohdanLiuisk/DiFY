using System.Security.Claims;
using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.IdentityServer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dify.Core.Application.Users.Commands;

public record AuthenticateCommand(
    string Login, 
    string Password
) : IRequest<AuthenticationResult>;

public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticationResult> 
{
    private readonly IDifyContext _difyContext;

    public AuthenticateCommandHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }
    
    public async Task<AuthenticationResult> Handle(AuthenticateCommand command, CancellationToken cancellationToken)
    {
        var user = await _difyContext.Users
            .FirstOrDefaultAsync(u => u.Login == command.Login, cancellationToken);
        if (user == null)
        {
            return new AuthenticationResult("Incorrect login.");
        }
        if (!PasswordHashManager.VerifyHashedPassword(user.Password, command.Password))
        {
            return new AuthenticationResult("Incorrect password.");
        }
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.Name, user.Name),
            new(CustomClaimTypes.Login, user.Login),
            new(CustomClaimTypes.Email, user.Email),
            new(CustomClaimTypes.UserId, user.Id.ToString()),
        };
        var authenticateUserDto = new AuthenticatedUserDto(user.Id, user.Login, user.Email, user.Password, claims);
        return new AuthenticationResult(authenticateUserDto);
    }
}
