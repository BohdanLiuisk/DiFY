using Dify.Core.Infrastructure.Context;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Dify.Core.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseIdentityDb(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
    }
    
    public static void UseDifyCoreContext(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<DifyContextInitializer>();
        initializer.Initialise();
    }
}
