using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Participating;

public class Participant : Entity, IAggregateRoot
{
    public ParticipantId Id { get; private set; }
}