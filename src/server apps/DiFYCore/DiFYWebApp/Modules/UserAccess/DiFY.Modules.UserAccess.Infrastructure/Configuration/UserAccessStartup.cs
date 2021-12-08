using Autofac;
using DiFY.BuildingBlocks.Application;
using DiFY.Modules.UserAccess.Infrastructure.Configuration.DataAccess;
using DiFY.Modules.UserAccess.Infrastructure.Configuration.Domain;
using DiFY.Modules.UserAccess.Infrastructure.Configuration.Logging;
using DiFY.Modules.UserAccess.Infrastructure.Configuration.Mediation;
using Serilog;
using Serilog.AspNetCore;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration
{
    public class UserAccessStartup
    {
        private static IContainer _container;

        public static void Initialize(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger)
        {
            ConfigureCompositionRoot(connectionString, executionContextAccessor, logger);
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "UserAccess")));

            var loggerFactory = new SerilogLoggerFactory(logger);

            containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterModule(new )
            containerBuilder.RegisterModule(new MediatorModule());

            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();

            UserAccessCompositionRoot.SetContainer(_container);
        }
    }
}