using Dify.Core.Domain.Common;
using Dify.Core.Domain.Enums;
using Dify.Core.Domain.Events;

namespace Dify.Core.Domain.Entities;

public class CallParticipant : BaseEntity<int>
{
    public Guid CallId { get; set; }
    
    public Call Call { get; set; }

    public int ParticipantId { get; set; }
    
    public User Participant { get; set; }
    
    public DateTime JoinedAt { get; set; }
    
    public bool Active { get; set; }
    
    public string StreamId { get; set; }
    
    public string PeerId { get; set;  }
    
    public string ConnectionId { get; set; }
    
    public CallDirection Direction { get; set; }
    
    public CallParticipantStatus Status { get; set; }

    public void DeclineCall()
    {
        Status = CallParticipantStatus.Declined;
        Active = false;
        AddDomainEvent(new ParticipantDeclinedCall(CallId, ParticipantId));
    }

    public void LeftCall()
    {
        Active = false;
        StreamId = string.Empty;
        PeerId = string.Empty;
        ConnectionId = string.Empty;
        Status = CallParticipantStatus.NotActive;
        AddDomainEvent(new ParticipantLeftCallEvent(this));
    }
}
