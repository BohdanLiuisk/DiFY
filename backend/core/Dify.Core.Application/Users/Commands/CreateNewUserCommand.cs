using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.IdentityServer;
using Dify.Core.Domain.Entities;

namespace Dify.Core.Application.Users.Commands;

public record CreateNewUserCommand(
    string FirstName,
    string LastName,
    string Login, 
    string Password, 
    string Email
) : IRequest<CommandResponse<NewUserResponse>>;

public record NewUserResponse(int UserId);

public class CreateNewUserCommandHandler : IRequestHandler<CreateNewUserCommand, CommandResponse<NewUserResponse>> 
{
    private readonly IDifyContext _difyContext;

    public CreateNewUserCommandHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }

    public async Task<CommandResponse<NewUserResponse>> Handle(CreateNewUserCommand command, 
        CancellationToken cancellationToken)
    {
        if (await _difyContext.Users.AnyAsync(u => u.Login == command.Login, cancellationToken))
        {
            throw new ArgumentException($"User with login {command.Login} already exists");
        }
        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Name = $"{command.FirstName} {command.LastName}",
            Login = command.Login,
            Password = command.Password,
            Email = command.Email
        };
        var password = PasswordHashManager.HashPassword(command.Password);
        user.Password = password;
        await _difyContext.Users.AddAsync(user, cancellationToken);
        await _difyContext.SaveChangesAsync(cancellationToken);
        return new CommandResponse<NewUserResponse>(new NewUserResponse(user.Id));
    }
}
