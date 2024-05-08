using Dify.Entity.Migrations.Models;
using Microsoft.EntityFrameworkCore;

namespace Dify.Entity.Extensions;

public class DifyEntityOptions
{
    public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }
    
    public Func<Task<IEnumerable<TableDescriptor>>>? LoadTablesFromOuterStore { get; set; }
    
    public string? ConnectionString { get; set; }
}
