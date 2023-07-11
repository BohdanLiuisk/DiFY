﻿using System.Security.Claims;
using Dify.Core.Application.Common.Interfaces;
using Dify.Core.Application.IdentityServer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dify.Core.Application.Users.Commands.AuthenticateCommand;

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
            new(CustomClaimTypes.Name, user.Login),
            new(CustomClaimTypes.Email, user.Email)
        };
        var authenticateUserDto = new AuthenticatedUser(user.Id, user.Login, user.Email, claims , user.Password);
        return new AuthenticationResult(authenticateUserDto);
    }
}
