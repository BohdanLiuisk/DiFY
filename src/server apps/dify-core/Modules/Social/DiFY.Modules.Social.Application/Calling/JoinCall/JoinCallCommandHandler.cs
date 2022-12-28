using System;
using System.Threading;
using System.Threading.Tasks;
using DiFY.Modules.Social.Application.Configuration.Commands;
using DiFY.Modules.Social.Domain.Calling;
using DiFY.Modules.Social.Domain.Calling.Abstractions;
using DiFY.Modules.Social.Domain.Membership.Abstraction;
using MediatR;

namespace DiFY.Modules.Social.Application.Calling.JoinCall;

public class JoinCallCommandHandler : ICommandHandler<JoinCallCommand>
{
    private readonly IMemberContext _memberContext;

    private readonly ICallRepository _callRepository;

    public JoinCallCommandHandler(ICallRepository callRepository, IMemberContext memberContext)
    {
        _callRepository = callRepository;
        _memberContext = memberContext;
    }
    
    public async Task<Unit> Handle(JoinCallCommand command, CancellationToken cancellationToken)
    {
        var call = await _callRepository.GetByIdAsync(new CallId(command.CallId));
        call.Join(_memberContext.MemberId, command.StreamId, command.PeerId, command.ConnectionId, DateTime.UtcNow);
        return Unit.Value;
    }
}