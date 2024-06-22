using Dify.Entity.Abstract;
using Dify.Entity.Context;
using Dify.Entity.DbEngine;
using Dify.Entity.Initialization;
using Dify.Entity.Structure;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Dify.Entity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDifyEntity(this IServiceCollection services, 
        Action<DifyEntityOptions>? difyEntityOptionsAction = null) {
        var difyEntityOptions = new DifyEntityOptions();
        difyEntityOptionsAction?.Invoke(difyEntityOptions);
        services.AddSingleton(difyEntityOptions);
        services.AddDbContext<DifyEntityContext>(difyEntityOptions.ConfigureDbContext);
        services.AddFluentMigratorCore().ConfigureRunner(rb => rb.AddPostgres()
            .WithGlobalConnectionString(difyEntityOptions.ConnectionString));
        services.AddScoped<EntityStructureManager>();
        services.AddTransient<IEntityDbEngine, EntityDbEngine>();
        services.AddHostedService<DifyEntityHostedService>();
    }
}
