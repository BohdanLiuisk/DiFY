using Dify.Core.Application.Common;
using Dify.Core.Domain.Entities;
using Dify.Core.Infrastructure.Context.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Dify.Core.Infrastructure.Context;

public class DifyContext : DbContext, IDifyContext
{
    public DbSet<User> Users { get; set; }

    public DifyContext(DbContextOptions<DifyContext> options) : base(options) 
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
    }
}
