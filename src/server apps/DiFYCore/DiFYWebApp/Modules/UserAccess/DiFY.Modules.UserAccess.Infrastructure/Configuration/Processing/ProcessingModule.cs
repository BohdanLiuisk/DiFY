using Autofac;
using DiFY.BuildingBlocks.Infrastructure;
using DiFY.BuildingBlocks.Infrastructure.DomainEventDispatching;
using DiFY.BuildingBlocks.Infrastructure.Interfaces;
using DiFY.Modules.UserAccess.Application.Configuration.Commands;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    public class ProcessingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DomainEventsDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainNotificationsMapper>()
                .As<IDomainNotificationsMapper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventsAccessor>()
                .As<IDomainEventsAccessor>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterDecorator(
                typeof(UnitOfWorkCommandHandlerDecorator<>), 
                typeof(ICommandHandler<>));
            
        }
    }
}