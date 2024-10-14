using Dify.Common.Dto;
using Dify.Core.Application.Calls.Commands;
using Dify.Core.Application.Calls.Queries;
using Dify.Core.Application.Common;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Dify.Core.WebApi.Hubs;

public class CallHub : Hub
{
    private readonly IMediator _mediator;

    private readonly ICurrentUser _currentUser;
    
    private static readonly Dictionary<string, Guid> _connectionToCall = new();
    
    private static readonly Dictionary<string, int> _connectionToParticipant = new();
    
    public CallHub(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }
    
    [HubMethodName("OnJoinCall")]
    public async Task<CurrentCallDto> JoinCall(JoinCallCommand joinCall)
    {
        joinCall.ConnectionId = Context.ConnectionId;
        await _mediator.Send(joinCall);
        return (await _mediator.Send(new GetCurrentCallByIdQuery(joinCall.CallId))).Body;
    }
    
    [HubMethodName("OnParticipantJoined")]
    public async Task ParticipantJoined(Guid callId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, callId.ToString());
        _connectionToCall.Remove(Context.ConnectionId);
        _connectionToCall.Add(Context.ConnectionId, callId);
        _connectionToParticipant.Remove(Context.ConnectionId);
        _connectionToParticipant.Add(Context.ConnectionId, _currentUser.UserId);
        var call = (await _mediator.Send(new GetCurrentCallByIdQuery(callId))).Body;
        var participant = call.Participants.FirstOrDefault(p => p.ParticipantId == _currentUser.UserId);
        await Clients.GroupExcept(callId.ToString(), Context.ConnectionId)
            .SendAsync("OnParticipantJoined", participant);
    }
    
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var callId = _connectionToCall[Context.ConnectionId];
        _connectionToCall.Remove(Context.ConnectionId);
        await _mediator.Send(new LeftCallCommand(callId));
        await Clients.Group(callId.ToString())
            .SendAsync("OnParticipantLeft", new
            {
                ParticipantId = _connectionToParticipant[Context.ConnectionId]
            });
        await base.OnDisconnectedAsync(exception);
    }
}
