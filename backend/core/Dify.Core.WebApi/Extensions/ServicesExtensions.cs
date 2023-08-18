using Dify.Core.Application.Common;
using Dify.Core.Application.Services;
using Dify.Core.WebApi.Services;
using Microsoft.OpenApi.Models;

namespace Dify.Core.WebApi.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwagger();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddControllers();
        services.AddCors(o => o.AddPolicy("CorsPolicy", b =>
        {
            b.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins(configuration["ClientAppOrigin"]);
        }));
        services.AddSignalR();
        services.AddTransient<IDifyNotificationService, DifyNotificationService>();
        return services;
    }
    
    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
        return services;
    }
}
