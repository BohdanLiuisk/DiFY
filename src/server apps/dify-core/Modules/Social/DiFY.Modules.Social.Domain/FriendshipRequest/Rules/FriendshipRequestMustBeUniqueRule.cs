using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.FriendshipRequests.Delegates;

namespace DiFY.Modules.Social.Domain.FriendshipRequests.Rules
{
    public class FriendshipRequestMustBeUniqueRule : IBusinessRule
    {
        private readonly CountActiveFriendshipRequests _countActiveFriendshipRequests;

        private readonly RequesterId _requesterId;

        private readonly AddresseeId _addresseeId;

        public FriendshipRequestMustBeUniqueRule(
            CountActiveFriendshipRequests countActiveFriendshipRequests, 
            RequesterId requesterId, 
            AddresseeId addresseeId) 
        {
            _countActiveFriendshipRequests = countActiveFriendshipRequests;
            _requesterId = requesterId;
            _addresseeId = addresseeId;
        }
        
        public string Message => "You have already sent friendship request.";

        public bool IsBroken() => _countActiveFriendshipRequests.Invoke(_requesterId.Value, _addresseeId.Value) >= 1;
    }
}
