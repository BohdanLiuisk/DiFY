using Autofac;
using DiFY.Modules.Social.Application.FriendshipRequest;
using DiFY.Modules.Social.Domain.FriendshipRequests.Interfaces;

namespace DiFY.Modules.Social.Infrastructure.Configuration.Domain
{
    internal class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FriendshipRequestService>()
                .As<IFriendshipRequestService>()
                .InstancePerLifetimeScope();
        }
    }
}
