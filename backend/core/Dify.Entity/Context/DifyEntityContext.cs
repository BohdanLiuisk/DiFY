using Dify.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Dify.Entity.Context;

public class DifyEntityContext(DbContextOptions<DifyEntityContext> options) : DbContext(options)
{
    public DbSet<EntityDescriptor> EntityDescriptor { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        modelBuilder.ApplyConfiguration(new EntityDescriptorTypeConfig());
    }
}
