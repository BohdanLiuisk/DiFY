using System.Collections.Generic;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.BuildingBlocks.Infrastructure.Interfaces
{
    public interface IDomainEventsAccessor
    {
        IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

        void ClearAllDomainEvents();
    }
}