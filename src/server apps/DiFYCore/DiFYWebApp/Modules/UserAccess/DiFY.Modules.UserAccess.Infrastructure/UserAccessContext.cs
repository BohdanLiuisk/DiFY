using DiFY.Modules.UserAccess.Domain.UserRegistrations;
using DiFY.Modules.UserAccess.Domain.Users;
using DiFY.Modules.UserAccess.Infrastructure.Domain.UserRegistrations;
using DiFY.Modules.UserAccess.Infrastructure.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiFY.Modules.UserAccess.Infrastructure
{
    public class UserAccessContext : DbContext
    {
        public DbSet<UserRegistration> UserRegistrations { get; set; }
        
        public DbSet<User> Users { get; set; }

        private readonly ILoggerFactory _loggerFactory;

        public UserAccessContext(DbContextOptions options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserRegistrationEntityTypeConfiguration());
        }
    }
}