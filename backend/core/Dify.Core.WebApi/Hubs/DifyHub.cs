using Dify.Core.Application.Calls.Commands;
using Dify.Core.Application.Common;
using Dify.Core.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Dify.Core.WebApi.Hubs;

public class DifyHub : Hub
{
    private readonly IMediator _mediator;

    private readonly ICurrentUser _currentUser;
    
    public DifyHub(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await _mediator.Send(new UserConnectedCommand(Context.ConnectionId, _currentUser.UserId));
    }
    
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
        await _mediator.Send(new UserDisconnectedCommand(Context.ConnectionId));
    }
    
    [HubMethodName("OnDeclineIncomingCall")]
    public async Task DeclineIncomingCall(DeclineIncomingCallCommand command)
    {
        await _mediator.Send(command);
    }
}
