using System.Threading.Tasks;

namespace DiFY.BuildingBlocks.Infrastructure.Interfaces
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}