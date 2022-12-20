using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application;
using Microsoft.AspNetCore.SignalR;

namespace DiFY.WebAPI.Modules.Social;

public class SocialHub : Hub
{
    private readonly IExecutionContextAccessor _contextAccessor;
    
    public SocialHub(IExecutionContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", new
        {
            user, 
            message, 
            userId = _contextAccessor.UserId
        });
    }
}