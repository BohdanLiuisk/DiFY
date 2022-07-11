using Autofac;
using DiFY.BuildingBlocks.Application;
using DiFY.Modules.UserAccess.Infrastructure.Configuration.DataAccess;
using DiFY.Modules.UserAccess.Infrastructure.Configuration.Domain;
using DiFY.Modules.UserAccess.Infrastructure.Configuration.EventBus;
using DiFY.Modules.UserAccess.Infrastructure.Configuration.Logging;
using DiFY.Modules.UserAccess.Infrastructure.Configuration.Mediation;
using DiFY.Modules.UserAccess.Infrastructure.Configuration.Processing;
using Serilog;
using Serilog.AspNetCore;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration
{
    public class UserAccessStartup
    {
        private static IContainer _container;

        public static void Initialize(
            string connectionString,
            string eventBusConnection,
            string userAccessQueue,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger)
        {
            var moduleLogger = logger.ForContext("Module", "UserAccess");
            
            ConfigureCompositionRoot(connectionString, eventBusConnection, userAccessQueue, executionContextAccessor, logger);
            
            EventsBusStartup.Initialize(moduleLogger);
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            string eventBusConnection,
            string userAccessQueue,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "UserAccess")));

            var loggerFactory = new SerilogLoggerFactory(logger);

            containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterModule(new ProcessingModule());
            containerBuilder.RegisterModule(new EventsBusModule(eventBusConnection, userAccessQueue));
            containerBuilder.RegisterModule(new MediatorModule());
            
            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();

            UserAccessCompositionRoot.SetContainer(_container);
        }
    }
}