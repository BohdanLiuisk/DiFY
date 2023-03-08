using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application.Data;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DiFY.BuildingBlocks.Infrastructure.Redis;

public class RedisDb : IRedisDb
{
    private readonly IDatabase _database;
    
    public RedisDb(IDatabase database)
    {
        _database = database;
    }
    
    public async Task SetStringAsync(string key, string value)
    {
        await _database.StringSetAsync(key, value);
    }

    public async Task DeleteKeyAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    } 
    
    public async Task<string> GetStringAsync(string key)
    {
        return await _database.StringGetAsync(key);
    }
    
    public async Task SetJsonAsync<T>(string key, T value)
    {
        await _database.StringSetAsync(key, JsonConvert.SerializeObject(value));
    }

    public async Task<T> GetJsonAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        return value.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(value);
    }
}