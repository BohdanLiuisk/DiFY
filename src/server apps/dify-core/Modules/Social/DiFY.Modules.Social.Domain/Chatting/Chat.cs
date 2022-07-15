using System.Collections.Generic;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.Participating;

namespace DiFY.Modules.Social.Domain.Chatting;

public class Chat : Entity, IAggregateRoot
{
    public ChatId Id { get; private set; }
    
    private readonly List<Participant> _participants = new();
    
    public IReadOnlyList<Participant> Participants => _participants.AsReadOnly();
}