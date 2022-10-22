using System.Threading;
using System.Threading.Tasks;
using DiFY.Modules.Social.Application.Configuration.Commands;
using DiFY.Modules.Social.Domain.Calling;
using DiFY.Modules.Social.Domain.Calling.Abstractions;
using DiFY.Modules.Social.Domain.Membership.Abstraction;

namespace DiFY.Modules.Social.Application.Calling.EndCall;

public class EndCallCommandHandler : ICommandHandler<EndCallCommand, CallSummary>
{
    private readonly ICallRepository _callRepository;

    private readonly IMemberContext _memberContext;

    public EndCallCommandHandler(ICallRepository callRepository, IMemberContext memberContext)
    {
        _callRepository = callRepository;
        _memberContext = memberContext;
    }
    
    public async Task<CallSummary> Handle(EndCallCommand command, CancellationToken cancellationToken)
    {
        var call = await _callRepository.GetByIdAsync(new CallId(command.CallId));
        call.End(command.EndDate, _memberContext.MemberId);
        return new CallSummary
        {
            Duration = call.GetDuration(),
            ParticipantsCount = call.Participants.Count
        };
    }
}