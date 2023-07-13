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
        });
        return services;
    }
}
