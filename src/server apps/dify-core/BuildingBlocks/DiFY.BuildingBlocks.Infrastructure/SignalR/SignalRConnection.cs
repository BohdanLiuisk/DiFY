using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace DiFY.BuildingBlocks.Infrastructure.SignalR;

public class SignalRConnection : ISignalRConnection
{
    private readonly HubConnection _hubConnection;
    
    public SignalRConnection(HubConnection hubConnection)
    {
        _hubConnection = hubConnection;
    }
    
    public async Task InvokeAsync(string methodName, object arg1)
    {
        await _hubConnection.InvokeAsync(methodName, arg1);
    }
}