using System;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application.Data;
using StackExchange.Redis;

namespace DiFY.BuildingBlocks.Infrastructure.Redis;

public class RedisConnectionFactory : IRedisConnectionFactory, IDisposable
{
    private ConnectionMultiplexer _redis;

    private readonly string _redisHost;

    public RedisConnectionFactory(string redisHost)
    {
        _redisHost = redisHost;
    }
    
    public IRedisDb GetConnection()
    {
        var configString = $"{_redisHost}";
        if (_redis is { IsConnected: true }) return new RedisDb(_redis.GetDatabase());
        _redis = ConnectionMultiplexer.Connect(configString);
        return new RedisDb(_redis.GetDatabase());
    }
    
    public async Task<IRedisDb> GetConnectionAsync()
    {
        var configString = $"{_redisHost}";
        if (_redis is { IsConnected: true }) return new RedisDb(_redis.GetDatabase());
        _redis = await ConnectionMultiplexer.ConnectAsync(configString);
        return new RedisDb(_redis.GetDatabase());
    }

    public void Dispose()
    {
        if (_redis.IsConnected)  
        {
            _redis.Dispose();   
        }
    }
}