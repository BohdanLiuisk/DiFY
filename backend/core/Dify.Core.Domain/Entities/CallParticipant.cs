using Dify.Core.Domain.Common;
using Dify.Core.Domain.Enums;

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
}
