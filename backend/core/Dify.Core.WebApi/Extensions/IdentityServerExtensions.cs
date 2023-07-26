using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Dify.Core.WebApi.Extensions;

public static class IdentityServerExtensions
{
    public static IServiceCollection AddIdentityServerAuthentication(this IServiceCollection services)
    {
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
            o.Events = new JwtBearerEvents
            {
                OnMessageReceived = HandleOnMessageReceived
            };
        });
        return services;
    }
    
    private static Task HandleOnMessageReceived(MessageReceivedContext context)
    {
        var path = context.HttpContext.Request.Path;
        var accessToken = context.Request.Query["access_token"];
        var isHub = path.StartsWithSegments("/hubs/dify");
        if (!string.IsNullOrEmpty(accessToken) && isHub)
        {
            context.Token = accessToken;
        }
        return Task.CompletedTask;
    }
}
