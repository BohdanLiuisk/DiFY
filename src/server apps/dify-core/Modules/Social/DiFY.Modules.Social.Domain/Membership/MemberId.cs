using System;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Membership;

public class MemberId : TypedIdValueBase
{
    public MemberId(Guid value) : base(value) { }
}