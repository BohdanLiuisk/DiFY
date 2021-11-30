using Autofac;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.BuildingBlocks.Infrastructure;
using Microsoft.Extensions.Logging;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration.DataAccess
{
    internal class DataAccessModule : Autofac.Module
    {
        private readonly string _dbConnectionString;
        private readonly ILoggerFactory _loggerFactory;

        internal DataAccessModule(string dbConnectionString, ILoggerFactory loggerFactory)
        {
            _dbConnectionString = dbConnectionString;
            _loggerFactory = loggerFactory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", _dbConnectionString)
                .InstancePerLifetimeScope();
            
            
        }
    }
}