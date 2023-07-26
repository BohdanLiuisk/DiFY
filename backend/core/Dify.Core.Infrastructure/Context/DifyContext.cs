using Dify.Core.Application.Common;
using Dify.Core.Domain.Entities;
using Dify.Core.Infrastructure.Context.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Dify.Core.Infrastructure.Context;

public class DifyContext : DbContext, IDifyContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Call> Calls { get; set; }
    
    public DbSet<CallParticipant> CallParticipants { get; set; }

    public DifyContext(DbContextOptions<DifyContext> options) : base(options) 
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CallEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CallParticipantEntityTypeConfiguration());
    }
}
