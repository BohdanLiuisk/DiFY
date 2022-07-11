using Autofac;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace DiFY.Modules.Social.Infrastructure.Configuration.DataAccess
{
    internal class DataAccessModule : Module
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
            builder.Register(c =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<SocialContext>();
                dbContextOptionsBuilder.UseSqlServer(_dbConnectionString);
                dbContextOptionsBuilder
                    .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
                return new SocialContext(dbContextOptionsBuilder.Options, _loggerFactory);

            })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope();
            var infrastructureAssembly = typeof(SocialContext).Assembly;
            builder.RegisterAssemblyTypes(infrastructureAssembly)
                .Where(type => type.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .FindConstructorsWith(new AllConstructorFinder());
        }
    }
}
