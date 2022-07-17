using DiFY.Modules.Social.Domain.Calling;
using DiFY.Modules.Social.Domain.FriendshipRequests;
using DiFY.Modules.Social.Domain.Friendships;
using DiFY.Modules.Social.Domain.Membership;
using DiFY.Modules.Social.Infrastructure.Domain.Calling;
using DiFY.Modules.Social.Infrastructure.Domain.FriendshipRequests;
using DiFY.Modules.Social.Infrastructure.Domain.Friendships;
using DiFY.Modules.Social.Infrastructure.Domain.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiFY.Modules.Social.Infrastructure
{
    public class SocialContext : DbContext 
    {
        private readonly ILoggerFactory _loggerFactory;
        
        public DbSet<Member> Members { get; set; }
        
        public DbSet<Call> Calls { get; set; }

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
            modelBuilder.ApplyConfiguration(new CallEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MemberEntityTypeConfiguration());
        }
    }
}
