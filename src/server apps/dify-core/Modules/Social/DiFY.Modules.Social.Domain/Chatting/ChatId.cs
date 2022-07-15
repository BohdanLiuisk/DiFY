using System;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Chatting;

public class ChatId : TypedIdValueBase
{
    protected ChatId(Guid value) : base(value) { }
}
