using System;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Calling;

public class CallId : TypedIdValueBase
{
    protected CallId(Guid value) : base(value) { }
}