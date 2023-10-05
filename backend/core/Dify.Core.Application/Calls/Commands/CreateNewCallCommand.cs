using Dify.Common.Dto.Call;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Services;
using Dify.Core.Domain.Entities;

namespace Dify.Core.Application.Calls.Commands;

public record CreateNewCallCommand(
    string Name,
    IEnumerable<int> ParticipantIds
): IRequest<CommandResponse<NewCallResponse>>;

public record NewCallResponse(Guid CallId);

public record Participant(int Id, string ConnectionId, string Name);

public class CreateNewCallCommandHandler : IRequestHandler<CreateNewCallCommand, CommandResponse<NewCallResponse>>
{
    private readonly IDifyContext _difyContext;
    
    private readonly IDifyNotificationService _difyNotificationService;
    
    private readonly ICurrentUser _currentUser;

    public CreateNewCallCommandHandler(IDifyContext difyContext, 
        IDifyNotificationService difyNotificationService, ICurrentUser currentUser)
    {
        _difyContext = difyContext;
        _difyNotificationService = difyNotificationService;
        _currentUser = currentUser;
    }

    public async Task<CommandResponse<NewCallResponse>> Handle(CreateNewCallCommand command, CancellationToken cancellationToken)
    {
        var call = Call.CreateNew(command.Name);
        await _difyContext.Calls.AddAsync(call, cancellationToken);
        await _difyContext.SaveChangesAsync(cancellationToken);
        var participants = await _difyContext.Users.Where(
                u => (command.ParticipantIds.Contains(u.Id) && u.Online && !string.IsNullOrEmpty(u.ConnectionId)) 
                     || u.Id == _currentUser.UserId)
            .Select(u => new Participant(u.Id, u.ConnectionId, u.Name))
            .ToListAsync(cancellationToken);
        var currentUser = participants.FirstOrDefault(p => p.Id == _currentUser.UserId);
        var participantsToCall = participants.Where(p => p.Id != _currentUser.UserId).ToList();
        foreach (var participant in participantsToCall)
        {
            await _difyNotificationService.SendIncomingCallEventAsync(new IncomingCallEventDto(
                call.Id, call.Name, currentUser.Id, currentUser.Name), participant.ConnectionId);
        }
        return new CommandResponse<NewCallResponse>(new NewCallResponse(call.Id));
    }
}
