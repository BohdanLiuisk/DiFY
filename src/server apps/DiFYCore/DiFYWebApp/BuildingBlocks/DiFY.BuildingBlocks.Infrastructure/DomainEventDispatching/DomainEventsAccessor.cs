using System.Collections.Generic;
using System.Linq;
using DiFY.BuildingBlocks.Domain;
using DiFY.BuildingBlocks.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiFY.BuildingBlocks.Infrastructure.DomainEventDispatching
{
    public class DomainEventsAccessor : IDomainEventsAccessor
    {
        private readonly DbContext _context;

        public DomainEventsAccessor(DbContext context)
        {
            _context = context;
        }

        public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
        {
            var domainEntities = _context
                .ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            return domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();
        }

        public void ClearAllDomainEvents()
        {
            var domainEntities = _context
                .ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();
            
            domainEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());
        }
    }
}