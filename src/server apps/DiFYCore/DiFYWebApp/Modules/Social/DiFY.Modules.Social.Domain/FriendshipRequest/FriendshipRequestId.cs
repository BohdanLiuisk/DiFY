using DiFY.BuildingBlocks.Domain;
using System;

namespace DiFY.Modules.Social.Domain.FriendshipRequest
{
    public class FriendshipRequestId : TypedIdValueBase
    {
        public FriendshipRequestId(Guid value) : base(value) { }
    }
}
