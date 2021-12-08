using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace DiFY.WebAPI
{
    public class Program
    {
        private static IConfigurationRoot Configuration { get; } = GetConfigurationRoot();
        
        public static void Main(string[] args)
        {
            ConfigureLogging();
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("https://*:5050");
                });
        
        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(Configuration))
                .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            Log.Logger.Information("Serilog configuration for DiFY is finished.");
        }
        
        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration)
        {
            return new(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"core.logs"
            };
        }
        
        private static IConfigurationRoot GetConfigurationRoot()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables();
            
            if (environment == Environments.Development)
            {
                configurationBuilder.AddUserSecrets(typeof(Program).Assembly);
            }
            
            return configurationBuilder.Build();
        }
    }
}