using DiFY.BuildingBlocks.Domain;
using System;

namespace DiFY.Modules.Social.Domain.FriendshipRequests
{
    public class FriendshipRequestId : TypedIdValueBase
    {
        public FriendshipRequestId(Guid value) : base(value) { }
    }
}
