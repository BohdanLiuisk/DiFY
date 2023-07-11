using Dify.Core.Infrastructure.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Dify.Core.Application.Common.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DifyContext>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("DifyCoreDb"),
                     builder => builder.MigrationsAssembly(typeof(DifyContext).Assembly.FullName)));
        services.AddScoped<IDifyContext>(provider => provider.GetRequiredService<DifyContext>());
        services.AddScoped<DifyContextInitializer>();
        return services;
    }
}
