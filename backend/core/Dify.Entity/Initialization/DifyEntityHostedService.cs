using Dify.Entity.Descriptor;
using Dify.Entity.Extensions;
using Microsoft.Extensions.Hosting;

namespace Dify.Entity.Initialization;

internal class DifyEntityHostedService(DifyEntityOptions difyEntityOptions, 
        EntityStructureManager entityStructureManager) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken) {
        IList<TableDescriptor>? tableDescriptors = null;
        if (difyEntityOptions.LoadTablesFromOuterStore != null) {
            tableDescriptors = (await difyEntityOptions.LoadTablesFromOuterStore()).ToList();
        }
        var initConfig = new EntityStructureInitConfig {
            OuterTables = tableDescriptors
        };
        await entityStructureManager.InitializeAsync(initConfig);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }
}
