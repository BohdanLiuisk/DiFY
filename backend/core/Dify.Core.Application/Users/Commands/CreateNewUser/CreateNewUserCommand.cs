using MediatR;

namespace Dify.Core.Application.Users.Commands.CreateNewUser;

public record CreateNewUserCommand(
    string FirstName,
    string LastName,
    string Login, 
    string Password, 
    string Email
) : IRequest<int>;
