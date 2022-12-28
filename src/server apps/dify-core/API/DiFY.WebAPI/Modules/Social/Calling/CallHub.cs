using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application;
using DiFY.Modules.Social.Application.Calling;
using DiFY.Modules.Social.Application.Calling.GetCall;
using DiFY.Modules.Social.Application.Calling.GetCallParticipant;
using DiFY.Modules.Social.Application.Calling.JoinCall;
using DiFY.Modules.Social.Application.Calling.LeftCall;
using DiFY.Modules.Social.Application.Contracts;
using DiFY.WebAPI.Modules.Social.Calling.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace DiFY.WebAPI.Modules.Social.Calling;

public class CallHub : Hub
{
    private readonly ISocialModule _socialModule;

    private readonly IExecutionContextAccessor _contextAccessor;
    
    private static readonly Dictionary<string, Guid> _connectionToCall = new();
    
    private static readonly Dictionary<string, Guid> _connectionToParticipant = new();
    
    public CallHub(ISocialModule socialModule, IExecutionContextAccessor contextAccessor)
    {
        _socialModule = socialModule;
        _contextAccessor = contextAccessor;
    }  
    
    [HubMethodName("OnJoinCall")]
    public async Task<GetCallQueryResult> JoinCall(JoinCallDto joinCall)
    {
        await _socialModule.ExecuteCommandAsync(
            new JoinCallCommand(joinCall.CallId, joinCall.StreamId, joinCall.PeerId, Context.ConnectionId));
        return await _socialModule.ExecuteQueryAsync(new GetCallQuery(joinCall.CallId));
    }
    
    [HubMethodName("OnParticipantJoined")]
    public async Task ParticipantJoined(Guid callId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, callId.ToString());
        _connectionToCall.Remove(Context.ConnectionId);
        _connectionToCall.Add(Context.ConnectionId, callId);
        _connectionToParticipant.Remove(Context.ConnectionId);
        _connectionToParticipant.Add(Context.ConnectionId, _contextAccessor.UserId);
        var participant = await _socialModule.ExecuteQueryAsync(
            new GetCallParticipantQuery(_contextAccessor.UserId));
        await Clients.GroupExcept(callId.ToString(), Context.ConnectionId)
            .SendAsync("OnParticipantJoined", participant);
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var callId = _connectionToCall[Context.ConnectionId];
        _connectionToCall.Remove(Context.ConnectionId);
        await _socialModule.ExecuteCommandAsync(new LeftCallCommand(callId));
        await Clients.Group(callId.ToString())
            .SendAsync("OnParticipantLeft", new { ParticipantId = _connectionToParticipant[Context.ConnectionId]});
        await base.OnDisconnectedAsync(exception);
    }
}
