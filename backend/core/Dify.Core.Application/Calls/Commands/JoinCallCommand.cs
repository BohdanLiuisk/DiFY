using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Exceptions;
using Dify.Core.Domain.Entities;

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

    private readonly IMapper _mapper;
    
    public JoinCallCommandHandler(IDifyContext difyContext, ICurrentUser currentUser, IMapper mapper)
    {
        _difyContext = difyContext;
        _currentUser = currentUser;
        _mapper = mapper;
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
            if (participant.Active)
            {
                throw new AggregateException("You are already in a call.");
            }
            participant.Active = true;
            participant.StreamId = command.StreamId;
            participant.PeerId = command.PeerId;
            participant.ConnectionId = command.ConnectionId;
        }
        else
        {
            var newParticipant = _mapper.Map<CallParticipant>(command);
            newParticipant.JoinedAt = DateTime.UtcNow;
            newParticipant.Active = true;
            newParticipant.ParticipantId = _currentUser.UserId;
            call.Participants.Add(newParticipant);
        }
        await _difyContext.SaveChangesAsync(cancellationToken);
        return new CommandResponse<bool>(true);
    }
}
