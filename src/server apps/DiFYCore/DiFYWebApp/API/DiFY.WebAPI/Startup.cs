using Autofac;
using Autofac.Extensions.DependencyInjection;
using DiFY.BuildingBlocks.Application;
using DiFY.BuildingBlocks.Domain.Exceptions;
using DiFY.Modules.Administration.Infrastructure.Configuration;
using DiFY.Modules.UserAccess.Application.IdentityServer;
using DiFY.Modules.UserAccess.Infrastructure.Configuration;
using DiFY.WebAPI.Configuration.Authorization;
using DiFY.WebAPI.Configuration.ExecutionContext;
using DiFY.WebAPI.Configuration.Extensions;
using DiFY.WebAPI.Configuration.Validation;
using DiFY.WebAPI.Modules.Administration;
using DiFY.WebAPI.Modules.UserAccess;
using Hellang.Middleware.ProblemDetails;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace DiFY.WebAPI
{
    public class Startup
    {
        private const string DiFyConnectionString = "ConnectionStrings:DiFYConnectionString";
        
        private const string EventBusConnection = "RabbitMQConfiguration:Uri";

        private const string UserAccessQueue = "RabbitMQConfiguration:Queues:UserAccess";

        private const string AdministrationQueue = "RabbitMQConfiguration:Queues:Administration";
        
        private static ILogger _logger;
        
        private static ILogger _loggerForApi;
        
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration _, IHostEnvironment env)
        {
            ConfigureLogger();
            
            _configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true)
                .AddEnvironmentVariables()
                .Build();
            
            _loggerForApi.Information("Connection string:" + _configuration[DiFyConnectionString]);
            
            AuthorizationChecker.CheckAllEndpoints();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddSwaggerDocumentation();
            
            ConfigureIdentityServer(services);
            
            services.AddHttpContextAccessor();
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
                {
                    policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
                    policyBuilder.AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme);
                });
            });

            services.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
        }
        
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new AdministrationAutofacModule());

            containerBuilder.RegisterModule(new UserAccessAutofacModule());
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var container = app.ApplicationServices.GetAutofacRoot();
            
            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            
            InitializeModules(container);
            
            app.UseMiddleware<CorrelationMiddleware>();
            
            app.UseSwaggerDocumentation();
            
            app.UseIdentityServer();
            
            if (env.IsDevelopment())
            {
                app.UseProblemDetails();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureIdentityServer(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddInMemoryPersistedGrants()
                .AddProfileService<ProfileService>()
                .AddDeveloperSigningCredential();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, x =>
                {
                    x.Authority = "http://localhost:5050";
                    x.ApiName = "DiFYCoreAPI";
                    x.RequireHttpsMetadata = false;
                });
        }
        
        private static void ConfigureLogger()
        {
            _logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.RollingFile(new CompactJsonFormatter(), "logs/logs")
                .CreateLogger();

            _loggerForApi = _logger.ForContext("Module", "API");

            _loggerForApi.Information("Logger configured");
        }
        
        private void InitializeModules(ILifetimeScope container)
        {
            var httpContextAccessor = container.Resolve<IHttpContextAccessor>();

            var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);

            AdministrationStartup.Initialize(
                _configuration[DiFyConnectionString],
                _configuration[EventBusConnection],
                _configuration[AdministrationQueue],
                executionContextAccessor,
                _logger);

            UserAccessStartup.Initialize(
                _configuration[DiFyConnectionString],
                _configuration[EventBusConnection],
                _configuration[UserAccessQueue],
                executionContextAccessor,
                _logger);
        }
    }
}