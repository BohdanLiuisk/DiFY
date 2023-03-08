using System;

namespace DiFY.Modules.Social.Application.Members;

public record GetUserProfileDto(Guid Id, string Login, string Email, string FirstName, string LastName, 
    DateTime CreatedOn, string AvatarUrl);
