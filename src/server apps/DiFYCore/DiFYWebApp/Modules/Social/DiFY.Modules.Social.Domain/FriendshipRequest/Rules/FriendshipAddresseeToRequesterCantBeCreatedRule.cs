using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.FriendshipRequests.Delegates;

namespace DiFY.Modules.Social.Domain.FriendshipRequests.Rules
{
    public class FriendshipAddresseeToRequesterCantBeCreatedRule : IBusinessRule
    {
        private readonly CountActiveFriendshipRequests _countActiveFriendshipRequests;

        private readonly AddresseeId _addresseeId;

        private readonly RequesterId _requesterId;

        public FriendshipAddresseeToRequesterCantBeCreatedRule(
            CountActiveFriendshipRequests countActiveFriendshipRequests, 
            AddresseeId addresseeId,
            RequesterId requesterId)
        {
            _countActiveFriendshipRequests = countActiveFriendshipRequests;
            _addresseeId = addresseeId;
            _requesterId = requesterId;
        }

        public string Message => "This user has already sent friendship request for you.";
        
        public bool IsBroken() => _countActiveFriendshipRequests.Invoke(_addresseeId.Value, _requesterId.Value) >= 1;
    }
}
