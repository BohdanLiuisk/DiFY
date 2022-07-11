using System;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Administration.Domain.Users
{
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value) : base(value) { }
    }
}