using DiFY.Modules.Social.Application.Contracts;
using System;

namespace DiFY.Modules.Social.Application.FriendshipRequest.RejectFriendshipRequest
{
    public class RejectFriendshipRequestCommand : CommandBase
    {
        public RejectFriendshipRequestCommand(Guid friendshipRequestId)
        {
            FriendshipRequestId = friendshipRequestId;
        }

        public Guid FriendshipRequestId { get; }
    }
}
