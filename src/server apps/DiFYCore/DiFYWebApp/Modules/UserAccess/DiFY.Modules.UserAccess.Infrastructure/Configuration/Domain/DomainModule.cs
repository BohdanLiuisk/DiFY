using Autofac;
using DiFY.Modules.UserAccess.Application.UserRegistrations.Services;
using DiFY.Modules.UserAccess.Domain.UserRegistrations.Interfaces;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UsersCounter>()
                .As<IUsersCounter>()
                .InstancePerLifetimeScope();
        }
    }
}