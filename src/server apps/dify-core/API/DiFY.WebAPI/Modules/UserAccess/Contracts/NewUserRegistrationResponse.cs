using System;

namespace DiFY.WebAPI.Modules.UserAccess.Contracts;

public record NewUserRegistrationResponse()
{
    public Guid NewUserId { get; set; }
};