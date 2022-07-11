using Autofac;
using DiFY.Modules.Administration.Application.Contracts;
using DiFY.Modules.Administration.Infrastructure;

namespace DiFY.WebAPI.Modules.Administration
{
    internal class AdministrationAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdministrationModule>()
                .As<IAdministrationModule>()
                .InstancePerLifetimeScope();
        }
    }
}