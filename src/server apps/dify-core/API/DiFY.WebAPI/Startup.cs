using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DiFY.BuildingBlocks.Application;
using DiFY.BuildingBlocks.Domain.Exceptions;
using DiFY.Modules.Administration.Infrastructure.Configuration;
using DiFY.Modules.Social.Infrastructure.Configuration;
using DiFY.Modules.UserAccess.Application.IdentityServer;
using DiFY.Modules.UserAccess.Infrastructure.Configuration;
using DiFY.WebAPI.Configuration.Authorization;
using DiFY.WebAPI.Configuration.ExecutionContext;
using DiFY.WebAPI.Configuration.Extensions;
using DiFY.WebAPI.Configuration.Validation;
using DiFY.WebAPI.Modules.Administration;
using DiFY.WebAPI.Modules.Social;
using DiFY.WebAPI.Modules.Social.Calling;
using DiFY.WebAPI.Modules.UserAccess;
using Hellang.Middleware.ProblemDetails;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace DiFY.WebAPI
{
    public class Startup
    {
        private const string DiFyConnectionString = "ConnectionStrings:DiFYConnectionString";
        
        private const string RedisHostConnectionString = "ConnectionStrings:RedisHost";
        
        private const string SignalRConnectionString = "ConnectionStrings:SignalR";
        
        private const string EventBusConnection = "RabbitMQConfiguration:Uri";

        private const string UserAccessQueue = "RabbitMQConfiguration:Queues:UserAccess";

        private const string SocialQueue = "RabbitMQConfiguration:Queues:Social";

        private const string AdministrationQueue = "RabbitMQConfiguration:Queues:Administration";
        
        private const string TestOrigin = "TestOrigin";
        
        private const string ClientAppOrigin = "ClientAppOrigin";
        
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
            AppContext.SetSwitch(
                "Microsoft.AspNetCore.Authorization.SuppressUseHttpContextAsAuthorizationResource",
                isEnabled: true);
            services.AddControllers();
            services.AddCors(o => o.AddPolicy("CorsPolicy", b =>
            {
                b.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins(
                    _configuration[TestOrigin],
                    _configuration[ClientAppOrigin]);
            }));
            services.AddSingleton<ICorsPolicyService>((container) => {
                var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
                return new DefaultCorsPolicyService(logger)
                {
                    AllowAll = true
                };
            });
            services.AddSwaggerDocumentation();
            ConfigureIdentityServer(services);
            services.AddHttpContextAccessor();
            services.AddSignalR();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => 
                    new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex =>
                    new BusinessRuleValidationExceptionProblemDetails(ex));
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
            containerBuilder.RegisterModule(new SocialAutofacModule());
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeIdentityDb(app);
            var container = app.ApplicationServices.GetAutofacRoot();
            app.UseCors("CorsPolicy");
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DifyHub>("/hubs/dify");
                endpoints.MapHub<CallHub>("/hubs/call");
            });
        }

        private void ConfigureIdentityServer(IServiceCollection services)
        {
            var identityMigrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddOperationalStore(options => 
                    options.ConfigureDbContext = b => 
                        b.UseSqlServer(_configuration[DiFyConnectionString],
                        sql => sql.MigrationsAssembly(identityMigrationsAssembly)))
                .AddProfileService<ProfileService>()
                .AddDeveloperSigningCredential();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = 
                    JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = 
                    JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = "http://localhost:5050";
                o.Audience = "DiFYCoreAPI";
                o.RequireHttpsMetadata = false;
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var path = context.HttpContext.Request.Path;
                        var accessToken = context.Request.Query["access_token"];
                        var isHub = path.StartsWithSegments("/hubs/call") || path.StartsWithSegments("/hubs/dify");
                        if (!string.IsNullOrEmpty(accessToken) && isHub)
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        private static void InitializeIdentityDb(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
            serviceScope?.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
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
            SocialStartup.Initialize(
                _configuration[DiFyConnectionString],
                _configuration[RedisHostConnectionString],
                _configuration[SignalRConnectionString],
                _configuration[EventBusConnection],
                _configuration[SocialQueue],
                executionContextAccessor,
                _logger);
        }
    }
}