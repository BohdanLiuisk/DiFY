using System.Collections.Generic;
using System.Linq;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.Membership;

namespace DiFY.Modules.Social.Domain.Calling.Rules;

public class ExistingActiveParticipantCantJoinRule : IBusinessRule
{
    private readonly IEnumerable<CallParticipant> _callParticipants;

    private readonly MemberId _memberId;

    public ExistingActiveParticipantCantJoinRule(IEnumerable<CallParticipant> callParticipants, MemberId memberId)
    {
        _callParticipants = callParticipants;
        _memberId = memberId;
    }

    public bool IsBroken() => _callParticipants.Any(p => p.ParticipantId == _memberId && p.IsActive());

    public string Message => "You are already joined this call.";
}