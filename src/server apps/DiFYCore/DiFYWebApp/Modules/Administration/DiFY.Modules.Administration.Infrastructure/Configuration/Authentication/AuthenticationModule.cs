using Autofac;
using DiFY.Modules.Administration.Domain.Users;
using DiFY.Modules.Administration.Infrastructure.Configuration.Users;

namespace DiFY.Modules.Administration.Infrastructure.Configuration.Authentication
{
    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserContext>()
                .As<IUserContext>()
                .InstancePerLifetimeScope();
        }
    }
}
