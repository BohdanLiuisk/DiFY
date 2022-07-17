using System;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Calling;

public class CallId : TypedIdValueBase
{
    public CallId(Guid value) : base(value) { }
}