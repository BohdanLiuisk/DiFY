using Dify.Core.Application.Users.Commands.AuthenticateCommand;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MediatR;

namespace Dify.Core.Infrastructure.Auth;

public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly IMediator _mediator;
    
    public ResourceOwnerPasswordValidator(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var authenticationResult = await _mediator.Send(new AuthenticateCommand(context.UserName, context.Password));
        if (!authenticationResult.IsAuthenticated)
        {
            context.Result = new GrantValidationResult(
                TokenRequestErrors.InvalidGrant, authenticationResult.AuthenticationError);
            return;
        }
        context.Result = new GrantValidationResult(
            subject: authenticationResult.User.Id.ToString(),
            authenticationMethod: "forms",
            claims: authenticationResult.User.Claims);
    }
}
