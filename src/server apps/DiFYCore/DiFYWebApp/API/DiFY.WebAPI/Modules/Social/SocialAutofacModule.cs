using Autofac;
using DiFY.Modules.Social.Application.Contracts;
using DiFY.Modules.Social.Infrastructure;

namespace DiFY.WebAPI.Modules.Social
{
    public class SocialAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SocialModule>()
                .As<ISocialModule>()
                .InstancePerLifetimeScope();
        }
    }
}
