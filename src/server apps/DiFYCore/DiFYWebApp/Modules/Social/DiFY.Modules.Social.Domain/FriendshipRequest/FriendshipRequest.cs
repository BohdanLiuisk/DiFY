using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.FriendshipRequest.Events;
using DiFY.Modules.Social.Domain.FriendshipRequest.Interfaces;
using DiFY.Modules.Social.Domain.FriendshipRequest.Rules;
using DiFY.Modules.Social.Domain.Friendships;
using System;

namespace DiFY.Modules.Social.Domain.FriendshipRequest
{
    public class FriendshipRequest : Entity, IAggregateRoot
    {
        public FriendshipRequestId Id { get; private set; }

        private RequesterId _requesterId;

        private AddresseeId _addresseeId;

        private FriendshipRequestStatus _status;

        private DateTime _createdDate;

        private DateTime? _confirmedDate;

        private DateTime? _rejectedDate;

        private FriendshipRequest() { }

        private FriendshipRequest(
            RequesterId requesterId,
            AddresseeId addresseeId, 
            DateTime createdDate, 
            IFriendshipRequestService friendshipRequestService)
        {
            CheckRule(new FriendshipRequestMustBeUniqueRule(friendshipRequestService, requesterId, addresseeId));

            CheckRule(new FriendshipAddreseeToRequesterCantBeCreatedRule(friendshipRequestService, addresseeId, requesterId));

            Id = new FriendshipRequestId(Guid.NewGuid());

            _requesterId = requesterId;

            _addresseeId = addresseeId;

            _createdDate = createdDate;
        }

        public static FriendshipRequest CreateNewFriendshipRequest(
            RequesterId requesterId, 
            AddresseeId addresseeId, 
            DateTime createdDate, 
            IFriendshipRequestService friendshipRequestService)
        {
            return new FriendshipRequest(requesterId, addresseeId, createdDate, friendshipRequestService);
        }

        public Friendship CreateFriendship()
        {
            CheckRule(new FriendshipCantBeCreatedWhenRequestIsNotConfirmedRule(_status));

            return Friendship.CreateFriendshipFromRequest(Id, _requesterId, _addresseeId, DateTime.Now);
        }

        public void Confirm(DateTime confirmedDate)
        {
            _confirmedDate = confirmedDate;

            AddDomainEvent(new FriendshipRequestConfirmedDomainEvent(Id));
        }

        public void Reject(DateTime rejectedDate)
        {
            _rejectedDate = rejectedDate;

            AddDomainEvent(new FriendshipRequestRejectedDomainEvent(Id));
        }
    }
}
