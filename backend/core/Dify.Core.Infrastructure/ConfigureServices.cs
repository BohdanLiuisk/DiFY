using Dify.Core.Application.Common;
using Dify.Core.Application.IdentityServer;
using Dify.Core.Infrastructure.Auth;
using Dify.Core.Infrastructure.Context;
using Dify.Core.Infrastructure.Context.Interceptors;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dify.Core.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddDbContext<DifyContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(configuration.GetConnectionString("DifyCoreDb"),
                builder => builder.MigrationsAssembly(typeof(DifyContext).Assembly.FullName));
        });
        services.AddScoped<IDifyContext>(provider => provider.GetRequiredService<DifyContext>());
        services.AddScoped<DifyContextInitializer>();
        services.AddIdentityServer()
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiResources(IdentityServerConfig.GetApis())
            .AddInMemoryClients(IdentityServerConfig.GetClients())
            .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
            .AddOperationalStore(options => 
                options.ConfigureDbContext = b => 
                    b.UseSqlServer(configuration.GetConnectionString("DifyCoreDb"),
                        sql => sql.MigrationsAssembly(typeof(DifyContext).Assembly.FullName)))
            .AddProfileService<ProfileService>()
            .AddDeveloperSigningCredential();
        services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
        return services;
    }
}
