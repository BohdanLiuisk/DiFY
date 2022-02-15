using DiFY.BuildingBlocks.Domain;
using System;

namespace DiFY.Modules.Social.Domain.FriendshipRequest.Rules
{
    public class FriendshipCantBeCreatedWhenRequestIsNotConfirmedRule : IBusinessRule
    {
        private readonly FriendshipRequestStatus _friendshipRequestStatus;

        public FriendshipCantBeCreatedWhenRequestIsNotConfirmedRule(FriendshipRequestStatus friendshipRequestStatus)
        {
            _friendshipRequestStatus = friendshipRequestStatus;
        }

        public string Message => "Friendship can't be created when friendship request is not confirmed.";

        public bool IsBroken() => _friendshipRequestStatus != FriendshipRequestStatus.Confirmed;
    }
}
