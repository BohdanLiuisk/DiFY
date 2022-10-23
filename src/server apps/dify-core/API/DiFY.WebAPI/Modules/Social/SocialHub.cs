using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DiFY.WebAPI.Modules.Social;

public class SocialHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}