using System.Threading.Tasks;

namespace DiFY.BuildingBlocks.Application.Data;

public interface IRedisDb
{
    Task SetAsync<T>(string key, T value);

    Task<T> GetAsync<T>(string key);
}