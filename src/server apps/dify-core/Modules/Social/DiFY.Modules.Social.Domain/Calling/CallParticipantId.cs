using System;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Calling;

public class CallParticipantId : TypedIdValueBase
{
    public CallParticipantId(Guid value) : base(value) { }
}