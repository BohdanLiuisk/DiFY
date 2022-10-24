using System.Collections.Generic;
using System.Linq;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Social.Domain.Membership;

namespace DiFY.Modules.Social.Domain.Calling.Rules;

public class NotParticipantCantLeftOrEndCallRule : IBusinessRule
{
    private readonly IEnumerable<CallParticipant> _callParticipants;

    private readonly MemberId _memberId;

    public NotParticipantCantLeftOrEndCallRule(IEnumerable<CallParticipant> callParticipants, MemberId memberId)
    {
        _callParticipants = callParticipants;
        _memberId = memberId;
    }
    
    public bool IsBroken() => _callParticipants.All(c => c.ParticipantId != _memberId || 
                                                         (c.ParticipantId == _memberId && !c.IsActive()));
    
    public string Message => "You can't leave a call that you're not connected to.";
}