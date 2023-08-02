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
    
    private readonly IMapper _mapper;

    public CreateNewUserCommandHandler(IDifyContext difyContext, IMapper mapper)
    {
        _difyContext = difyContext;
        _mapper = mapper;
    }

    public async Task<CommandResponse<NewUserResponse>> Handle(CreateNewUserCommand command, 
        CancellationToken cancellationToken)
    {
        if (await _difyContext.Users.AnyAsync(u => u.Login == command.Login, cancellationToken))
        {
            throw new ArgumentException($"User with login {command.Login} already exists");
        }
        var user = _mapper.Map<User>(command);
        var password = PasswordHashManager.HashPassword(command.Password);
        user.Password = password;
        await _difyContext.Users.AddAsync(user, cancellationToken);
        await _difyContext.SaveChangesAsync(cancellationToken);
        return new CommandResponse<NewUserResponse>(new NewUserResponse(user.Id));
    }
}
