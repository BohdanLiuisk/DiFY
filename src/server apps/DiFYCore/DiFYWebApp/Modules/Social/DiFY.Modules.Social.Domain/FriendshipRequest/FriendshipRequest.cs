using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.FriendshipRequests.Events;
using DiFY.Modules.Social.Domain.FriendshipRequests.Rules;
using DiFY.Modules.Social.Domain.Friendships;
using System;
using DiFY.Modules.Social.Domain.FriendshipRequests.Delegates;

namespace DiFY.Modules.Social.Domain.FriendshipRequests
{
    public class FriendshipRequest : Entity, IAggregateRoot
    {
        public FriendshipRequestId Id { get; private set; }

        private RequesterId _requesterId;

        private AddresseeId _addresseeId;

        private FriendshipRequestStatus _status;

        private DateTime _createdOn;

        private DateTime? _confirmedOn;

        private DateTime? _rejectedOn;
        
        private FriendshipRequest() { }

        private FriendshipRequest(RequesterId requesterId, AddresseeId addresseeId, 
            DateTime createdDate, CountActiveFriendshipRequests countActiveFriendshipRequests) 
        {
            CheckRule(new FriendshipRequestMustBeUniqueRule(
                countActiveFriendshipRequests, requesterId, addresseeId));
            CheckRule(new FriendshipAddresseeToRequesterCantBeCreatedRule(
                countActiveFriendshipRequests, addresseeId, requesterId));
            Id = new FriendshipRequestId(Guid.NewGuid());
            _requesterId = requesterId;
            _addresseeId = addresseeId;
            _createdOn = createdDate;
        }

        public static FriendshipRequest CreateNewFriendshipRequest(RequesterId requesterId,  AddresseeId addresseeId, 
            DateTime createdDate, CountActiveFriendshipRequests countActiveFriendshipRequests)
        {
            return new FriendshipRequest(requesterId, addresseeId, createdDate, countActiveFriendshipRequests);
        }

        public Friendship CreateFriendship()
        {
            CheckRule(new FriendshipCantBeCreatedWhenRequestIsNotConfirmedRule(_status));
            return Friendship.CreateFriendshipFromRequest(Id, _requesterId, _addresseeId, DateTime.UtcNow);
        }

        public void Confirm(DateTime confirmedDate)
        {
            _confirmedOn = confirmedDate;
            _status = FriendshipRequestStatus.Confirmed;
            AddDomainEvent(new FriendshipRequestConfirmedDomainEvent(Id));
        }

        public void Reject(DateTime rejectedDate)
        {
            _rejectedOn = rejectedDate;
            _status = FriendshipRequestStatus.Rejected;
            AddDomainEvent(new FriendshipRequestRejectedDomainEvent(Id));
        }
    }
}
