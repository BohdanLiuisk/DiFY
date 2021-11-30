using Autofac;
using DiFY.BuildingBlocks.Application;
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

            containerBuilder.RegisterModule(new MediatorModule());

            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();

            UserAccessCompositionRoot.SetContainer(_container);
        }
    }
}