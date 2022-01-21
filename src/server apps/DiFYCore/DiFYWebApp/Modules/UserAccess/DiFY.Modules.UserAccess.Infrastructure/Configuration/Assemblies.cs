using System.Reflection;
using DiFY.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(ConfirmUserRegistrationCommand).Assembly;
    }
}