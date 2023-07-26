using System.Reflection;
using Dify.Core.Application;
using Dify.Core.Infrastructure;
using Serilog;
using Dify.Core.WebApi.Extensions;
using Dify.Core.WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", reloadOnChange: true, optional: true)
    .AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetExecutingAssembly(),true)
    .Build();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices(builder.Configuration);
builder.Services.AddIdentityServerAuthentication();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

app.UseIdentityDb();
app.UseDifyCoreContext();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseIdentityServer();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<DifyHub>("/hubs/dify");
app.Run();
