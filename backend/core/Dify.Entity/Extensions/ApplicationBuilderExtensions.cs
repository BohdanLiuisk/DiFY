using Dify.Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dify.Entity.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseDifyEntity(this IHost app) 
    {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<DifyEntityContext>().Database.Migrate();
    }
}
