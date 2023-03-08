using System;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application;
using DiFY.Modules.Social.Application.Contracts;
using DiFY.Modules.Social.Application.UserEvents.UserConnected;
using DiFY.Modules.Social.Application.UserEvents.UserDisconnected;
using Microsoft.AspNetCore.SignalR;

namespace DiFY.WebAPI.Modules.Social;

public class DifyHub : Hub
{
    private readonly ISocialModule _socialModule;
    
    private readonly IExecutionContextAccessor _contextAccessor;
    
    public DifyHub(IExecutionContextAccessor contextAccessor, ISocialModule socialModule)
    {
        _contextAccessor = contextAccessor;
        _socialModule = socialModule;
    }

    public override async Task OnConnectedAsync()
    {
        await _socialModule.ExecuteCommandAsync(new UserConnectedCommand(_contextAccessor.UserId, Context.ConnectionId));
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await _socialModule.ExecuteCommandAsync(new UserDisconnectedCommand(_contextAccessor.UserId));
    }
}
