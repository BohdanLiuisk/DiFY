using DiFY.BuildingBlocks.Domain;
using System;

namespace DiFY.Modules.Social.Domain.FriendshipRequests
{
    public class AddresseeId : TypedIdValueBase
    {
        public AddresseeId(Guid value) : base(value) { }
    }
}
