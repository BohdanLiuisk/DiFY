using System.Collections.Generic;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.Membership;

namespace DiFY.Modules.Social.Domain.Chatting;

public class Chat : Entity, IAggregateRoot
{
    public ChatId Id { get; private set; }
    
    private readonly List<Member> _participants = new();
    
    public IReadOnlyList<Member> Participants => _participants.AsReadOnly();
}