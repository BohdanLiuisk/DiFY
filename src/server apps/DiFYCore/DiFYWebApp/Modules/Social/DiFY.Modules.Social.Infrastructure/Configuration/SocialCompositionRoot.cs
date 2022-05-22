using Autofac;

namespace DiFY.Modules.Social.Infrastructure.Configuration
{
    internal static class SocialCompositionRoot
    {
        private static IContainer _container;

        internal static void SetContainer(IContainer container)
        {
            _container = container;
        }

        internal static ILifetimeScope BeginLifetimeScope() => _container.BeginLifetimeScope();
    }
}
