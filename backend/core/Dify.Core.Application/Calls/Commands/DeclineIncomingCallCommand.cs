using Dify.Core.Application.Common;

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
        var callParticipant = await _difyContext.CallParticipants
            .FirstOrDefaultAsync(c => c.CallId == command.CallId && c.ParticipantId == _currentUser.UserId, 
                cancellationToken);
        callParticipant.DeclineCall();
        await _difyContext.SaveChangesAsync(cancellationToken);
    }
}
