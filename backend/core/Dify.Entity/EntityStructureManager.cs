using Dify.Entity.Extensions;
using Dify.Entity.Models;
using Microsoft.Extensions.Logging;

namespace Dify.Entity;

public class EntityStructureManager(DifyEntityOptions difyEntityOptions, ILogger<EntityStructureManager> logger)
{
    public Task InitializeAsync(EntityStructureInitConfig? initConfig)
    {
        logger.LogInformation("Dify entities initialization started");
        if (initConfig is { OuterTables: not null })
        {
            logger.LogInformation($"Outer tables count: {initConfig.OuterTables.Count}");
        }
        return Task.CompletedTask;
    }
}
