using Dify.Core.Application.Common.Interfaces;
using Dify.Core.Domain.Entities;
using MediatR;

namespace Dify.Core.Application.Users.Commands.CreateNewUser;

public class CreateNewUserCommandHandler : IRequestHandler<CreateNewUserCommand, int> 
{
    private readonly IDifyContext _difyContext;

    public CreateNewUserCommandHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }

    public async Task<int> Handle(CreateNewUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User {
            Login = command.UserName,
            Password = command.Password,
            Email = command.Email
        };
        await _difyContext.Users.AddAsync(user, cancellationToken);
        await _difyContext.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}
