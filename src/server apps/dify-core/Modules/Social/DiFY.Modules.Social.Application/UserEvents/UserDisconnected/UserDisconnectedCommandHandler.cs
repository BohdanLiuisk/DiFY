using System.Threading;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.Social.Application.Configuration.Commands;
using MediatR;

namespace DiFY.Modules.Social.Application.UserEvents.UserDisconnected;

public class UserDisconnectedCommandHandler : ICommandHandler<UserDisconnectedCommand>
{
    private readonly IRedisConnectionFactory _redisConnectionFactory;

    public UserDisconnectedCommandHandler(IRedisConnectionFactory redisConnectionFactory)
    {
        _redisConnectionFactory = redisConnectionFactory;
    }
    
    public async Task<Unit> Handle(UserDisconnectedCommand command, CancellationToken cancellationToken)
    {
        var redisDb = await _redisConnectionFactory.GetConnectionAsync();
        await redisDb.DeleteKeyAsync($"online-{command.UserId}");
        return Unit.Value;
    }
}
