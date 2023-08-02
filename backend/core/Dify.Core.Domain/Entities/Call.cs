using Dify.Core.Domain.Common;
using Dify.Core.Domain.Events;

namespace Dify.Core.Domain.Entities;

public class Call : BaseAuditableEntity<Guid>
{
    public string Name { get; set; }

    public int? DroppedById { get; set; }

    public User DroppedBy { get; set; }

    public bool Active { get; set; }

    public DateTime? EndDate { get; set; }

    public double Duration { get; set; }

    public ICollection<CallParticipant> Participants { get; private set; }

    private Call()
    {
        Participants = new List<CallParticipant>();
    }
    
    public static Call CreateNew(string name)
    {
        var call = new Call
        {
            Name = name,
            Active = true
        };
        call.AddDomainEvent(new NewCallCreatedEvent(call));
        return call;
    }
    
    public void Join(int participantId, string streamId, string peerId, string connectionId)
    {
        var participant = new CallParticipant
        {
            ParticipantId = participantId,
            StreamId = streamId,
            PeerId = peerId,
            ConnectionId = connectionId,
            JoinedAt = DateTime.UtcNow,
            Active = true
        };
        Participants.Add(participant);
    }
}
