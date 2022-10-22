using Autofac;
using DiFY.BuildingBlocks.Application.SignalR;
using DiFY.BuildingBlocks.Infrastructure.SignalR;

namespace DiFY.Modules.Social.Infrastructure.Configuration.SignalR;

internal class SignalRModule : Module
{
    private readonly string _connectionUrl;
        
    internal SignalRModule(string connectionUrl)
    {
        _connectionUrl = connectionUrl;
    }
        
    protected override void Load(ContainerBuilder builder) 
    {
        builder.RegisterType<SignalRConnectionFactory>()
            .As<ISignalRConnectionFactory>()
            .WithParameter("connectionUrl", _connectionUrl)
            .InstancePerLifetimeScope();
    }
}