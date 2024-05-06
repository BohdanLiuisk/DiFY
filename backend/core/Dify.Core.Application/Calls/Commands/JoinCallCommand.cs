using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Exceptions;
using Dify.Core.Domain.Enums;

namespace Dify.Core.Application.Calls.Commands;

public record JoinCallCommand : IRequest<CommandResponse<bool>>
{
    public Guid CallId { get; set; }
    public string StreamId { get; set; }
    public string PeerId { get; set; }
    public string ConnectionId { get; set; }
}

public class JoinCallCommandHandler : IRequestHandler<JoinCallCommand, CommandResponse<bool>>
{
    private readonly IDifyContext _difyContext;

    private readonly ICurrentUser _currentUser;
    
    public JoinCallCommandHandler(IDifyContext difyContext, ICurrentUser currentUser)
    {
        _difyContext = difyContext;
        _currentUser = currentUser;
    }

    public async Task<CommandResponse<bool>> Handle(JoinCallCommand command, CancellationToken cancellationToken)
    {
        var call = await _difyContext.Calls
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == command.CallId, cancellationToken);
        if (call is null)
        {
            throw new NotFoundException("You cannot join this call as it does not exist.");
        }
        if (!call.Active)
        {
            throw new AggregateException("You cannot join this call as it has ended.");
        }
        var participant = call.Participants
            .FirstOrDefault(p => p.ParticipantId == _currentUser.UserId);
        if (participant is not null)
        {
            if (participant.Status == CallParticipantStatus.Active)
            {
                throw new AggregateException("You are already in a call.");
            }
            participant.Status = CallParticipantStatus.Active;
            participant.StreamId = command.StreamId;
            participant.PeerId = command.PeerId;
            participant.ConnectionId = command.ConnectionId;
        }
        else
        {
            throw new AggregateException("You are not allowed to join this call.");
        }
        await _difyContext.SaveChangesAsync(cancellationToken);
        return new CommandResponse<bool>(true);
    }
}
