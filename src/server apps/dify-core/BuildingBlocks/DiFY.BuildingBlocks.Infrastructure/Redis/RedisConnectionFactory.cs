using System;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application.Data;
using StackExchange.Redis;

namespace DiFY.BuildingBlocks.Infrastructure.Redis;

public class RedisConnectionFactory : IRedisConnectionFactory, IDisposable
{
    private ConnectionMultiplexer _redis;

    private readonly string _redisHost;

    private readonly int _db;

    public RedisConnectionFactory(string redisHost, int db = 1)
    {
        _redisHost = redisHost;
        _db = db;
    }
    
    public IRedisDb GetConnection()
    {
        var configString = $"{_redisHost}";
        if (_redis is { IsConnected: true }) return new RedisDb(_redis.GetDatabase());
        _redis = ConnectionMultiplexer.Connect(configString);
        return new RedisDb(_redis.GetDatabase(_db));
    }
    
    public async Task<IRedisDb> GetConnectionAsync()
    {
        var configString = $"{_redisHost}";
        if (_redis is { IsConnected: true }) return new RedisDb(_redis.GetDatabase());
        _redis = await ConnectionMultiplexer.ConnectAsync(configString);
        return new RedisDb(_redis.GetDatabase(_db));
    }

    public void Dispose()
    {
        if (_redis.IsConnected)  
        {
            _redis.Dispose();   
        }
    }
}
