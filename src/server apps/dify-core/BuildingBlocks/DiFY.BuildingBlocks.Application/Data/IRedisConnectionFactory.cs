using System.Threading.Tasks;

namespace DiFY.BuildingBlocks.Application.Data;

public interface IRedisConnectionFactory
{
    IRedisDb GetConnection();

    Task<IRedisDb> GetConnectionAsync();
}