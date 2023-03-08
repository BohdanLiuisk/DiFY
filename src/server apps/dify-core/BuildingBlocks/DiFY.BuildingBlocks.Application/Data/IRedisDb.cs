using System.Threading.Tasks;

namespace DiFY.BuildingBlocks.Application.Data;

public interface IRedisDb
{
    Task SetStringAsync(string key, string value);
    
    Task<string> GetStringAsync(string key);
    
    Task SetJsonAsync<T>(string key, T value);

    Task<T> GetJsonAsync<T>(string key);

    Task DeleteKeyAsync(string key);
}