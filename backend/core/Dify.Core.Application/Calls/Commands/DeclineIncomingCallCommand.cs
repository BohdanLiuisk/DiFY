using Dify.Core.Application.Common;
using Dify.Core.Domain.Entities;

namespace Dify.Core.Application.Calls.Commands;

public record DeclineIncomingCallCommand(Guid CallId) : IRequest;

public class DeclineIncomingCallCommandHandler : IRequestHandler<DeclineIncomingCallCommand>
{
    private readonly IDifyContext _difyContext;

    private readonly ICurrentUser _currentUser;

    public DeclineIncomingCallCommandHandler(IDifyContext difyContext, ICurrentUser currentUser)
    {
        _difyContext = difyContext;
        _currentUser = currentUser;
    }
    
    public async Task Handle(DeclineIncomingCallCommand command, CancellationToken cancellationToken)
    {
        await _difyContext.CallParticipants.AddAsync(new CallParticipant()
        {
            CallId = command.CallId,
            ParticipantId = _currentUser.UserId,
            Active = false
        }, cancellationToken);
        await _difyContext.SaveChangesAsync(cancellationToken);
    }
}
