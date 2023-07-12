using System.Reflection;
using Dify.Core.Application.Common.Behaviours;
using Dify.Core.Application.MappingProfiles;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(QueryResponseHandlingBehavior<,>));
        });
        services.AddAutoMapper(typeof(UsersProfile));
        return services;
    }
}
