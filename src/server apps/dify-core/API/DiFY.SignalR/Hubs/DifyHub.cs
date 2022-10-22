using Microsoft.AspNetCore.SignalR;

namespace DiFY.SignalR.Hubs;

public class DifyHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        var value = Context?.User?.Claims.Single(x => x.Type == "sub").Value;
        if (value != null)
        {
            var userId = Guid.Parse(value);
            await Clients.All.SendAsync("ReceiveMessage", userId, message);
        }
    }
}