using Dify.Core.Domain.Common;
using Dify.Core.Domain.Entities;

namespace Dify.Core.Domain.Events;

public class NewCallCreatedEvent : BaseEvent
{
    public Call Call { get; }
    
    public NewCallCreatedEvent(Call call)
    {
        Call = call;
    }
}
