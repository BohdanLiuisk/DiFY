using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.UserEvents.UserDisconnected;

public class UserDisconnectedCommand : CommandBase
{
    public UserDisconnectedCommand(Guid userId)
    {
        UserId = userId;
    }
    
    public Guid UserId { get; }
}
