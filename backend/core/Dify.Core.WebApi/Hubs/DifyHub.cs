using Microsoft.AspNetCore.SignalR;

namespace Dify.Core.WebApi.Hubs;

public class DifyHub : Hub
{

    public DifyHub()
    {
    }

    public override async Task OnConnectedAsync()
    {
        
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        
    }
}
