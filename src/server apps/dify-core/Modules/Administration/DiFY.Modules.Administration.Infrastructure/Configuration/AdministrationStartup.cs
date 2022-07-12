using Autofac;
using DiFY.BuildingBlocks.Application;
using DiFY.Modules.Administration.Infrastructure.Configuration.Authentication;
using DiFY.Modules.Administration.Infrastructure.Configuration.DataAccess;
using DiFY.Modules.Administration.Infrastructure.Configuration.EventBus;
using DiFY.Modules.Administration.Infrastructure.Configuration.Logging;
using DiFY.Modules.Administration.Infrastructure.Configuration.Mediation;
using DiFY.Modules.Administration.Infrastructure.Configuration.Processing;
using Serilog;
using Serilog.AspNetCore;

namespace DiFY.Modules.Administration.Infrastructure.Configuration
{
    public static class AdministrationStartup
    {
        private static IContainer _container;
        
        public static void Initialize(
            string connectionString, 
            string eventBusConnection,
            string administrationQueue,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger)
        {
            var moduleLogger = logger.ForContext("Module", "Administration");
            ConfigureContainer(connectionString, eventBusConnection, administrationQueue, executionContextAccessor, moduleLogger);
            EventsBusStartup.Initialize(moduleLogger);
        }
        
        private static void ConfigureContainer(
            string connectionString,
            string eventBusConnection,
            string administrationQueue,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger));

            var loggerFactory = new SerilogLoggerFactory(logger);
            
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
            
            containerBuilder.RegisterModule(new ProcessingModule());
            
            containerBuilder.RegisterModule(new EventsBusModule(eventBusConnection, administrationQueue));
            
            containerBuilder.RegisterModule(new MediatorModule());
            
            containerBuilder.RegisterModule(new AuthenticationModule());
            
            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();

            AdministrationCompositionRoot.SetContainer(_container);
        }
    }
}