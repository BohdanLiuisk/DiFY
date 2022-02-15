using DiFY.BuildingBlocks.Domain;
using System;

namespace DiFY.Modules.Social.Domain.Friendships
{
    public class FriendshipId : TypedIdValueBase
    {
        public FriendshipId(Guid value) : base(value) { }
    }
}
