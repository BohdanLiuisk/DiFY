using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.FriendshipRequests.Interfaces;

namespace DiFY.Modules.Social.Domain.FriendshipRequests.Rules
{
    public class FriendshipAddreseeToRequesterCantBeCreatedRule : IBusinessRule
    {
        private readonly IFriendshipRequestService _friendshipService;

        private readonly AddresseeId _addresseeId;

        private readonly RequesterId _requesterId;

        public FriendshipAddreseeToRequesterCantBeCreatedRule(IFriendshipRequestService friendshipService, AddresseeId addresseeId, RequesterId requesterId)
        {
            _friendshipService = friendshipService;

            _addresseeId = addresseeId;

            _requesterId = requesterId;
        }

        public string Message => "This user has already sent friendship request for you.";
        
        public bool IsBroken() => _friendshipService.GetFriendshipRequestsCount(_addresseeId.Value, _requesterId.Value) > 1;
    }
}
