using System.Reflection;
using DiFY.Modules.UserAccess.Application.Configuration.Commands;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;
    }
}