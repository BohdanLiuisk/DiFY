using DiFY.BuildingBlocks.Domain;
using System;

namespace DiFY.Modules.Social.Domain.FriendshipRequest
{
    public class RequesterId : TypedIdValueBase
    {
        public RequesterId(Guid value) : base(value) { }
    }
}
