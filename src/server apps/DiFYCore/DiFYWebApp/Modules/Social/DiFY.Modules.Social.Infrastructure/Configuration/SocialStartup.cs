using Autofac;
using DiFY.BuildingBlocks.Application;
using DiFY.Modules.Social.Infrastructure.Configuration.DataAccess;
using DiFY.Modules.Social.Infrastructure.Configuration.Logging;
using DiFY.Modules.Social.Infrastructure.Configuration.EventBus;
using Serilog;
using Serilog.AspNetCore;
using DiFY.Modules.Social.Infrastructure.Configuration.Domain;
using DiFY.Modules.Social.Infrastructure.Configuration.Processing;
using DiFY.Modules.Social.Infrastructure.Configuration.Mediation;

namespace DiFY.Modules.Social.Infrastructure.Configuration
{
    public class SocialStartup
    {
        public static void Initialize(
           string connectionString,
           string eventBusConnection,
           string userAccessQueue,
           IExecutionContextAccessor executionContextAccessor,
           ILogger logger)
        {
            var moduleLogger = logger.ForContext("Module", "UserAccess");
            ConfigureCompositionRoot(connectionString, eventBusConnection, userAccessQueue, executionContextAccessor, logger);
            EventBusStartup.Initialize(moduleLogger);
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            string eventBusConnection,
            string userAccessQueue,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "Social")));
            var loggerFactory = new SerilogLoggerFactory(logger);
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterModule(new ProcessingModule());
            containerBuilder.RegisterModule(new EventsBusModule(eventBusConnection, userAccessQueue));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterInstance(executionContextAccessor);
            SocialCompositionRoot.SetContainer(containerBuilder.Build());
        }
    }
}
