using DiFY.BuildingBlocks.Domain;
using System;

namespace DiFY.Modules.Social.Domain.FriendshipRequests
{
    public class RequesterId : TypedIdValueBase
    {
        public RequesterId(Guid value) : base(value) { }
    }
}
