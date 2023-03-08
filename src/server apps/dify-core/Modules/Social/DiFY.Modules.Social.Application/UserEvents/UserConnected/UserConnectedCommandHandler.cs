using System.Threading;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.Social.Application.Configuration.Commands;
using MediatR;

namespace DiFY.Modules.Social.Application.UserEvents.UserConnected;

public class UserConnectedCommandHandler : ICommandHandler<UserConnectedCommand>
{
    private readonly IRedisConnectionFactory _redisConnectionFactory;
    
    public UserConnectedCommandHandler(IRedisConnectionFactory redisConnectionFactory)
    {
        _redisConnectionFactory = redisConnectionFactory;
    }
    
    public async Task<Unit> Handle(UserConnectedCommand command, CancellationToken cancellationToken)
    {
        var redisDb = await _redisConnectionFactory.GetConnectionAsync();
        await redisDb.SetStringAsync($"online-{command.UserId}", command.ConnectionId);
        return Unit.Value;
    }
}