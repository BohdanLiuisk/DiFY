using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application;
using DiFY.Modules.Social.Application.Calling.GetCallParticipant;
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
    
    [HubMethodName("OnParticipantJoined")]
    public async Task ParticipantJoined(CallConnectDto connectionData)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connectionData.CallId.ToString());
        _connectionToCall.Remove(Context.ConnectionId);
        _connectionToCall.Add(Context.ConnectionId, connectionData.CallId);
        _connectionToParticipant.Remove(Context.ConnectionId);
        _connectionToParticipant.Add(Context.ConnectionId, connectionData.UserId);
        connectionData.Participant = await _socialModule.ExecuteQueryAsync(
            new GetCallParticipantQuery(connectionData.UserId));
        await Clients.GroupExcept(connectionData.CallId.ToString(), Context.ConnectionId)
            .SendAsync("OnParticipantJoined", connectionData);
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
