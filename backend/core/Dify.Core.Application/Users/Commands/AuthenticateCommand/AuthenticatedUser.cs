using System.Security.Claims;

namespace Dify.Core.Application.Users.Commands.AuthenticateCommand;

public record AuthenticatedUser(
    int Id, 
    string Login, 
    string Email, 
    string Password, 
    List<Claim> Claims
);
