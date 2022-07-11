using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.FriendshipRequests.Events
{
    public class FriendshipRequestRejectedDomainEvent : DomainEventBase
    {
        public FriendshipRequestId FriendshipRequestId { get; }

        public FriendshipRequestRejectedDomainEvent(FriendshipRequestId friendshipRequestId)
        {
            FriendshipRequestId = friendshipRequestId;
        }
    }
}
