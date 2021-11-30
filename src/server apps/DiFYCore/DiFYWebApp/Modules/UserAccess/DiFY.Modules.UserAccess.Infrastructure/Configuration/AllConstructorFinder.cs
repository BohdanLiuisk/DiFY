using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Autofac.Core.Activators.Reflection;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration
{
    internal class AllConstructorFinder : IConstructorFinder
    {
        private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> Cache = new();

        public ConstructorInfo[] FindConstructors(Type targetType)
        {
            var result = Cache.GetOrAdd(
                targetType, type => type.GetTypeInfo().DeclaredConstructors.ToArray());
            
            return result.Length > 0 ? result : throw new NoConstructorsFoundException(targetType);
        }
    }
}