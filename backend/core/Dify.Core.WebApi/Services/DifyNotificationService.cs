using Dify.Common.Dto.Call;
using Dify.Core.Application.Services;
using Dify.Core.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Dify.Core.WebApi.Services;

public class DifyNotificationService : IDifyNotificationService
{
    private readonly IHubContext<DifyHub> _hubContext;
    
    public DifyNotificationService(IHubContext<DifyHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task SendIncomingCallEventAsync(IncomingCallEventDto incomingCallEventDto, string connectionId)
    {
        await _hubContext.Clients.Client(connectionId).SendAsync("OnIncomingCall", incomingCallEventDto, connectionId);
    }
}
