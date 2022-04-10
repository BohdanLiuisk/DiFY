using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.FriendshipRequests.Interfaces;

namespace DiFY.Modules.Social.Domain.FriendshipRequests.Rules
{
    public class FriendshipRequestMustBeUniqueRule : IBusinessRule
    {
        private readonly IFriendshipRequestService _friendshipRequestService;

        private readonly RequesterId _requesterId;

        private readonly AddresseeId _addreseeId;

        public FriendshipRequestMustBeUniqueRule(IFriendshipRequestService friendshipRequestService, RequesterId requesterId, AddresseeId addresseeId)
        {
            _friendshipRequestService = friendshipRequestService;

            _requesterId = requesterId;

            _addreseeId = addresseeId;
        }

        public string Message => "You have already sent friendship request.";

        public bool IsBroken() => _friendshipRequestService.GetFriendshipRequestsCount(_requesterId.Value, _addreseeId.Value) > 1;
    }
}
