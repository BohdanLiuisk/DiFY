using DiFY.Modules.Social.Domain.FriendshipRequests;
using DiFY.Modules.Social.Domain.Friendships;
using DiFY.Modules.Social.Infrastructure.Domain.FriendshipRequests;
using DiFY.Modules.Social.Infrastructure.Domain.Friendships;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiFY.Modules.Social.Infrastructure
{
    public class SocialContext : DbContext 
    {
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<FriendshipRequest> FriendshipRequests { get; set; }

        public SocialContext(DbContextOptions options, ILoggerFactory loggerFactory) : base(options) 
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FriendshipEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FriendshipRequestEntityTypeConfiguration());     
        }
    }
}
