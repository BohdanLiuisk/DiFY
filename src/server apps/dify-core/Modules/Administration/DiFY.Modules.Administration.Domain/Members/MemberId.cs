using System;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Administration.Domain.Members
{
    public class MemberId : TypedIdValueBase
    {
        public MemberId(Guid value) : base(value) { }
    }
}