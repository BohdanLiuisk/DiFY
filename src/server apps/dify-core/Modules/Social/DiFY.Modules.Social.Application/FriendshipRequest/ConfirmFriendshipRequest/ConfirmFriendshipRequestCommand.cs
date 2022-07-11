using DiFY.Modules.Social.Application.Contracts;
using System;

namespace DiFY.Modules.Social.Application.FriendshipRequest.ConfirmFriendshipRequest
{
    public class ConfirmFriendshipRequestCommand : CommandBase
    {
        public ConfirmFriendshipRequestCommand(Guid friendshipRequestId)
        {
            FriendshipRequestId = friendshipRequestId;
        }

        public Guid FriendshipRequestId { get; }
    }
}
