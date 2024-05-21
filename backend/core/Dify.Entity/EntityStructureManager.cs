using Dify.Entity.Abstract;
using Dify.Entity.Context;
using Dify.Entity.Descriptor;
using Dify.Entity.Initialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dify.Entity;

public class EntityStructureManager(IDbContextFactory<DifyEntityContext> dbContextFactory, IEntityDbEngine dbEngine,
    ILogger<EntityStructureManager> logger) 
{
    public async Task InitializeAsync(EntityStructureInitConfig? initConfig) {
        logger.LogInformation("Entity structure initialization started");
        var existingTables = await GetExistingTables();
        if (initConfig is { OuterTables: not null }) {
            var newTables = initConfig.OuterTables.Where(outerTable => existingTables
                .All(table => table.Id != outerTable.Id)).ToList();
            if (newTables.Count != 0) {
                logger.LogInformation($"Found {newTables.Count} new tables.");
                dbEngine.CreateNewTables(newTables);
            }
        }
    }

    private async Task<IList<TableDescriptor>> GetExistingTables() {
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        var entities = await dbContext.EntityDescriptor.ToListAsync();
        return entities.Select(e => e.ToTableDescriptor()).ToList();
    }
}
