using DiFY.Modules.Administration.Domain.Members;
using DiFY.Modules.Administration.Infrastructure.Domain.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiFY.Modules.Administration.Infrastructure
{
    public class AdministrationContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        
        internal DbSet<Member> Members { get; set; }
        
        public AdministrationContext(DbContextOptions options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MemberEntityTypeConfiguration());
        }
    }
}