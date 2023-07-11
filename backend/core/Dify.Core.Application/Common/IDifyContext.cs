using Dify.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dify.Core.Application.Common;

public interface IDifyContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
