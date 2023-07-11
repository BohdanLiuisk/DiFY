using AutoMapper;
using Dify.Core.Application.Common;
using Dify.Core.Application.IdentityServer;
using Dify.Core.Domain.Entities;
using MediatR;

namespace Dify.Core.Application.Users.Commands.CreateNewUser;

public class CreateNewUserCommandHandler : IRequestHandler<CreateNewUserCommand, int> 
{
    private readonly IDifyContext _difyContext;
    
    private readonly IMapper _mapper;

    public CreateNewUserCommandHandler(IDifyContext difyContext, IMapper mapper)
    {
        _difyContext = difyContext;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateNewUserCommand command, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(command);
        user.CreatedOn = DateTime.UtcNow;
        var password = PasswordHashManager.HashPassword(command.Password);
        user.Password = password;
        await _difyContext.Users.AddAsync(user, cancellationToken);
        await _difyContext.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}
