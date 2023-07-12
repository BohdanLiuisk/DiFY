using System.Security.Claims;

namespace Dify.Common.Dto;

public record AuthenticatedUserDto(
    int Id, 
    string Login, 
    string Email, 
    string Password, 
    List<Claim> Claims
);
