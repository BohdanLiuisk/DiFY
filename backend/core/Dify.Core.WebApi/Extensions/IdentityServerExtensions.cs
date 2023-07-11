using System.Reflection;
using Dify.Core.Application.IdentityServer;
using Dify.Core.Infrastructure.Context;
using Dify.Core.WebApi.Auth;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class IdentityServerExtensions
{
    public static IServiceCollection ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        var identityMigrationsAssembly = typeof(DifyContext).GetTypeInfo().Assembly.GetName().Name;
        services.AddIdentityServer()
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiResources(IdentityServerConfig.GetApis())
            .AddInMemoryClients(IdentityServerConfig.GetClients())
            .AddOperationalStore(options => 
                options.ConfigureDbContext = b => 
                    b.UseSqlServer(configuration.GetConnectionString("DifyCoreDb"),
                        sql => sql.MigrationsAssembly(identityMigrationsAssembly)))
            .AddProfileService<ProfileService>()
            .AddDeveloperSigningCredential();
        services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = 
                JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = 
                JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.Authority = "http://localhost:5065";
            o.Audience = "DiFYCoreAPI";
            o.RequireHttpsMetadata = false;
        });
        return services;
    }
}
