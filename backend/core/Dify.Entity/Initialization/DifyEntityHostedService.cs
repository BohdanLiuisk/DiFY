using Dify.Entity.Descriptor;
using Dify.Entity.Extensions;
using Dify.Entity.Structure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dify.Entity.Initialization;

internal class DifyEntityHostedService(DifyEntityOptions difyEntityOptions, IServiceProvider serviceProvider) 
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken) {
        IList<TableDescriptor>? tableDescriptors = null;
        if (difyEntityOptions.LoadTablesFromOuterStore != null) {
            tableDescriptors = (await difyEntityOptions.LoadTablesFromOuterStore()).ToList();
        }
        var initConfig = new EntityStructureInitConfig {
            OuterTables = tableDescriptors
        };
        using var scope = serviceProvider.CreateScope();
        var entityStructureManager = scope.ServiceProvider.GetRequiredService<EntityStructureManager>();
        await entityStructureManager.InitializeAsync(initConfig);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }
}
