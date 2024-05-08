using Dify.Entity.Extensions;
using Dify.Entity.Migrations.Models;
using Dify.Entity.Models;
using Microsoft.Extensions.Hosting;

namespace Dify.Entity.Initialization;

internal class DifyEntityHostedService : IHostedService
{
    private readonly DifyEntityOptions _difyEntityOptions;

    private readonly EntityStructureManager _entityStructureManager;
    
    public DifyEntityHostedService(DifyEntityOptions difyEntityOptions, EntityStructureManager entityStructureManager)
    {
        _difyEntityOptions = difyEntityOptions;
        _entityStructureManager = entityStructureManager;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        IList<TableDescriptor>? tableDescriptors = null;
        if (_difyEntityOptions.LoadTablesFromOuterStore != null)
        {
            tableDescriptors = (await _difyEntityOptions.LoadTablesFromOuterStore()).ToList();
        }
        var initConfig = new EntityStructureInitConfig
        {
            OuterTables = tableDescriptors
        };
        await _entityStructureManager.InitializeAsync(initConfig);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
