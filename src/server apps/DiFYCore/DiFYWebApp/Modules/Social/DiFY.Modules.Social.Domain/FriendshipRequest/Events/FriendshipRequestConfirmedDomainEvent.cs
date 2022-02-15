using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.FriendshipRequest.Events
{
    public class FriendshipRequestConfirmedDomainEvent : DomainEventBase
    {
        public FriendshipRequestId FriendshipRequestId { get; }

        public FriendshipRequestConfirmedDomainEvent(FriendshipRequestId friendshipRequestId)
        {
            FriendshipRequestId = friendshipRequestId;
        }
    }
}
