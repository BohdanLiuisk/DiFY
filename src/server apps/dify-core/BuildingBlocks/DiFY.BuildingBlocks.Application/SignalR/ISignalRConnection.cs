using System.Threading.Tasks;

namespace DiFY.BuildingBlocks.Application.SignalR;

public interface ISignalRConnection
{
    Task InvokeAsync(string methodName, object arg1);
}