using Dify.Entity.Context;
using Dify.Entity.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace Dify.Entity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDifyEntity(this IServiceCollection services, 
        Action<DifyEntityOptions>? difyEntityOptionsAction = null) 
    {
        var difyEntityOptions = new DifyEntityOptions();
        difyEntityOptionsAction?.Invoke(difyEntityOptions);
        services.AddSingleton(difyEntityOptions);
        services.AddDbContext<DifyEntityContext>(difyEntityOptions.ConfigureDbContext);
        services.AddSingleton<EntityStructureManager>();
        services.AddHostedService<DifyEntityHostedService>();
    }
}
