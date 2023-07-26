using Dify.Core.Domain.Entities;

namespace Dify.Core.Application.Common;

public interface IDifyContext
{
    DbSet<User> Users { get; }
    
    DbSet<Call> Calls { get; }
    
    DbSet<CallParticipant> CallParticipants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
