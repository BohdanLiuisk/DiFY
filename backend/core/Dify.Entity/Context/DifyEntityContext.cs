using Dify.Entity.Structure;
using Microsoft.EntityFrameworkCore;

namespace Dify.Entity.Context;

public class DifyEntityContext(DbContextOptions<DifyEntityContext> options) : DbContext(options)
{
    public DbSet<EntityStructure> EntityStructures { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfiguration(new EntityStructureTypeConfig());
    }
}
