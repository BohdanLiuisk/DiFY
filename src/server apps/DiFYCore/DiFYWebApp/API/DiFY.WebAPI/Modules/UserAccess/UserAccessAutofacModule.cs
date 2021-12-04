using Autofac;
using DiFY.Modules.UserAccess.Application.Contracts;
using DiFY.Modules.UserAccess.Infrastructure;

namespace DiFY.WebAPI.Modules.UserAccess
{
    public class UserAccessAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<UserAccessModule>()
                .As<IUserAccessModule>()
                .InstancePerLifetimeScope();
        }
    }
}