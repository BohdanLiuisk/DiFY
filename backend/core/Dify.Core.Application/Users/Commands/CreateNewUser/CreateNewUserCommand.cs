using MediatR;

namespace Dify.Core.Application.Users.Commands.CreateNewUser;

public record CreateNewUserCommand(string UserName, string Password, string Email) : IRequest<int>;
