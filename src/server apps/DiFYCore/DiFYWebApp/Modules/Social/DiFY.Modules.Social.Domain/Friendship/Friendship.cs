using System;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.FriendshipRequests;

namespace DiFY.Modules.Social.Domain.Friendships
{
    public class Friendship : Entity, IAggregateRoot
    {
        public FriendshipId Id { get; private set; }

        private RequesterId _requesterId;

        private AddresseeId _addresseeId;

        private DateTime _createdDate;

        private Friendship() { }

        private Friendship(
            FriendshipRequestId friendshipRequestId, 
            RequesterId requesterId,
            AddresseeId addresseeId, 
            DateTime createdDate)
        {
            Id = new FriendshipId(friendshipRequestId.Value);

            _requesterId = requesterId;

            _addresseeId = addresseeId;

            _createdDate = createdDate;
        }

        public static Friendship CreateFriendshipFromRequest(
            FriendshipRequestId friendshipRequestId, 
            RequesterId requesterId, 
            AddresseeId addresseeId, 
            DateTime createdDate)
        {
            return new Friendship(friendshipRequestId, requesterId, addresseeId, createdDate);
        }
    }
}
