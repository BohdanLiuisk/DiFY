using Autofac;
using DiFY.Modules.Social.Application.Members;
using DiFY.Modules.Social.Domain.Membership.Abstraction;

namespace DiFY.Modules.Social.Infrastructure.Configuration.Domain
{
    internal class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder) 
        {
            builder.RegisterType<MemberContext>()
                .As<IMemberContext>()
                .InstancePerLifetimeScope();
        }
    }
}
