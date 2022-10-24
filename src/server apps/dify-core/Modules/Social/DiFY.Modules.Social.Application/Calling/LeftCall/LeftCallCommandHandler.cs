using System.Threading;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.Social.Application.Configuration.Commands;
using DiFY.Modules.Social.Domain.Calling;
using DiFY.Modules.Social.Domain.Calling.Abstractions;
using DiFY.Modules.Social.Domain.Membership.Abstraction;
using MediatR;

namespace DiFY.Modules.Social.Application.Calling.LeftCall;

public class LeftCallCommandHandler : ICommandHandler<LeftCallCommand>
{
    private readonly IMemberContext _memberContext;

    private readonly ICallRepository _callRepository;
    
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public LeftCallCommandHandler(IMemberContext memberContext, ICallRepository callRepository, 
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _memberContext = memberContext;
        _callRepository = callRepository;
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<Unit> Handle(LeftCallCommand command, CancellationToken cancellationToken)
    {
        var call = await _callRepository.GetByIdAsync(new CallId(command.CallId));
        call.Left(_memberContext.MemberId);
        return Unit.Value;
    }
}