using Dify.Core.Domain.Common;
using Dify.Core.Domain.Enums;
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
    
    public static Call CreateNew(string name, int initiatorId, IEnumerable<int> participantIds)
    {
        var participants = new List<CallParticipant>();
        var initiatorParticipant = new CallParticipant
        {
            ParticipantId = initiatorId,
            Direction = CallDirection.Outgoing,
            Status = CallParticipantStatus.NotActive
        };
        participants.Add(initiatorParticipant);
        participants.AddRange(participantIds.Select(participantId => new CallParticipant
        {
            ParticipantId = participantId, 
            Direction = CallDirection.Incoming, 
            Status = CallParticipantStatus.NotAnswered
        }));
        var call = new Call
        {
            Name = name,
            Active = true,
            Participants = participants
        };
        call.AddDomainEvent(new NewCallCreatedEvent(call));
        return call;
    }

    public bool GetCanJoin(int participantId)
    {
        return Active || Participants.Any(p => p.ParticipantId == participantId && p.Active);
    }
}
