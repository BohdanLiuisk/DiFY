using System.Reflection;
using DiFY.Modules.Administration.Application.Contracts;

namespace DiFY.Modules.Administration.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(IAdministrationModule).Assembly;
    }
}