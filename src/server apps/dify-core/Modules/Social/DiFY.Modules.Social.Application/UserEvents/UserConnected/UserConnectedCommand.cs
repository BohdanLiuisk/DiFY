using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.UserEvents.UserConnected;

public class UserConnectedCommand : CommandBase
{
    public UserConnectedCommand(Guid userId, string connectionId)
    {
        UserId = userId;
        ConnectionId = connectionId;
    }
    
    public string ConnectionId { get; set; }

    public Guid UserId { get; }
}
