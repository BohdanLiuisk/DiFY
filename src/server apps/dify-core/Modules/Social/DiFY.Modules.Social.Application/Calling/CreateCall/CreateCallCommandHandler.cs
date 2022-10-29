using System;
using System.Threading;
using System.Threading.Tasks;
using DiFY.Modules.Social.Application.Configuration.Commands;
using DiFY.Modules.Social.Domain.Calling;
using DiFY.Modules.Social.Domain.Calling.Abstractions;
using DiFY.Modules.Social.Domain.Membership.Abstraction;

namespace DiFY.Modules.Social.Application.Calling.CreateCall;

internal class CreateCallCommandHandler : ICommandHandler<CreateCallCommand, Guid>
{
    private readonly ICallRepository _callRepository;

    private readonly IMemberContext _memberContext;

    public CreateCallCommandHandler(ICallRepository callRepository, IMemberContext memberContext)
    {
        _callRepository = callRepository;
        _memberContext = memberContext;
    }
    
    public async Task<Guid> Handle(CreateCallCommand command, CancellationToken cancellationToken)
    {
        var call = Call.CreateNew(command.Name, _memberContext.MemberId, command.StartDate);
        await _callRepository.AddAsync(call);
        return call.Id.Value;
    }
}