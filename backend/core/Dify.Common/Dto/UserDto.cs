namespace Dify.Common.Dto;

public record UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Name,
    string Login,
    string Email
);
