using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Exceptions;

namespace Dify.Core.Application.Calls.Commands;

public record LeftCallCommand(Guid CallId): IRequest<CommandResponse<bool>>;

public class LeftCallCommandHandler : IRequestHandler<LeftCallCommand, CommandResponse<bool>>
{
    private readonly IDifyContext _difyContext;

    private readonly ICurrentUser _currentUser;

    public LeftCallCommandHandler(IDifyContext difyContext, ICurrentUser currentUser)
    {
        _difyContext = difyContext;
        _currentUser = currentUser;
    }

    public async Task<CommandResponse<bool>> Handle(LeftCallCommand command, CancellationToken cancellationToken)
    {
        var call = await _difyContext.Calls
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == command.CallId, cancellationToken);
        if (call is null)
        {
            throw new NotFoundException("You cannot leave this call as it does not exist.");
        }
        if (!call.Active)
        {
            throw new AggregateException("Call is already ended.");
        }
        var participant = call.Participants.FirstOrDefault(
            p => p.ParticipantId == _currentUser.UserId && p.Active);
        if (participant is null)
        {
            throw new AggregateException("You cannot leave this call as you are not in it.");
        }
        participant.Active = false;
        await _difyContext.SaveChangesAsync(cancellationToken);
        return new CommandResponse<bool>(true);
    }
}
