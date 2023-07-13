using Dify.Core.Application.Common;
using Dify.Core.Application.IdentityServer;
using Dify.Core.Infrastructure.Auth;
using Dify.Core.Infrastructure.Context;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dify.Core.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DifyContext>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("DifyCoreDb"),
                     builder => builder.MigrationsAssembly(typeof(DifyContext).Assembly.FullName)));
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
