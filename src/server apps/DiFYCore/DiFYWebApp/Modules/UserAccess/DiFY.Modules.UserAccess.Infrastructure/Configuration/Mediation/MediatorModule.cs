using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration.Mediation
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterSource(new ScopedContravariantRegistrationSource(
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
                typeof(IValidator<>)));

            var mediatorOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
                typeof(IValidator<>)
            };

            foreach (var mediatorOpenType in mediatorOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(ThisAssembly, Assemblies.Application)
                    .AsClosedTypesOf(mediatorOpenType)
                    .AsImplementedInterfaces()
                    .FindConstructorsWith(new AllConstructorFinder());
            }

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var componentContext = ctx.Resolve<IComponentContext>();
                return type => componentContext.Resolve(type);
            }).InstancePerLifetimeScope();
        }
        
        private class ScopedContravariantRegistrationSource : IRegistrationSource
        {
            private readonly IRegistrationSource _source = new ContravariantRegistrationSource();
            private readonly List<Type> _types = new List<Type>();

            public ScopedContravariantRegistrationSource(params Type[] types)
            {
                if (types == null)
                {
                    throw new ArgumentException(nameof(types));
                }

                if (!types.All(x => x.IsGenericTypeDefinition))
                {
                    throw new AggregateException("Supplied types should be generic type definitions.");
                }
            
                _types.AddRange(types);
            }

            public IEnumerable<IComponentRegistration> RegistrationsFor
                (Service service, Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
            {
                var components = _source.RegistrationsFor(service, registrationAccessor);

                foreach (var component in components)
                {
                    var defs = component.Target.Services
                        .OfType<TypedService>()
                        .Select(x => x.ServiceType.GetGenericTypeDefinition());

                    if (defs.Any(_types.Contains))
                    {
                        yield return component;
                    }
                }
            }

            public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;
        }
    }
}