using Dify.Core.Application.Common;
using Dify.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Dify.Core.Infrastructure.Context.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;
    
    public AuditableEntityInterceptor(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }
    
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, 
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
        
    }

    private void UpdateEntities(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries().Where(e => e.Entity is IBaseAuditableEntity))
        {
            var auditableEntity = (IBaseAuditableEntity)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                if (_currentUser.IsAvailable)
                {
                    auditableEntity.CreatedById = _currentUser.UserId;
                }
                auditableEntity.CreatedOn = DateTime.UtcNow;
            }
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                if (_currentUser.IsAvailable)
                {
                    auditableEntity.ModifiedById = _currentUser.UserId;
                }
                auditableEntity.ModifiedOn = DateTime.UtcNow;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}
