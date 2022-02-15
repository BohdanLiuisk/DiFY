using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.FriendshipRequest.Interfaces;

namespace DiFY.Modules.Social.Domain.FriendshipRequest.Rules
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

        public bool IsBroken() => _friendshipService.AddresseeToRequesterExists(_addresseeId, _requesterId);
    }
}
