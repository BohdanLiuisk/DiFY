using System.Threading.Tasks;

namespace DiFY.BuildingBlocks.Application.SignalR;

public interface ISignalRConnectionFactory
{
    Task<ISignalRConnection> GetConnectionAsync(string hubName);
}