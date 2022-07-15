using System;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Participating;

public class ParticipantId : TypedIdValueBase
{
    protected ParticipantId(Guid value) : base(value) { }
}