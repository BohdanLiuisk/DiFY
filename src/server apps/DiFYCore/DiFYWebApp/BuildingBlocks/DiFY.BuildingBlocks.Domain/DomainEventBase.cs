using System;

namespace DiFY.BuildingBlocks.Domain
{
    public class DomainEventBase : IDomainEvent
    {
        public Guid Id { get; }
        
        public DateTime OccuredOn { get;  }

        public DomainEventBase()
        {
            Id = Guid.NewGuid();
            OccuredOn = DateTime.UtcNow;
        }
    }
}