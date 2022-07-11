using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace DiFY.WebAPI.Configuration.Authorization
{
    public static class AuthorizationChecker
    {
        public static void CheckAllEndpoints()
        {
            var assembly = typeof(Startup).Assembly;
            var allControllerTypes = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(ControllerBase)));
            var notProtectedActionMethods = (from controllerType in allControllerTypes
                let controllerHasPermissionAttribute = controllerType.GetCustomAttribute<HasPermissionAttribute>()
                where controllerHasPermissionAttribute == null
                let actionMethods = controllerType.GetMethods()
                    .Where(x => x.IsPublic && x.DeclaringType == controllerType)
                    .ToList()
                from publicMethod in actionMethods
                let hasPermissionAttribute = publicMethod.GetCustomAttribute<HasPermissionAttribute>()
                where hasPermissionAttribute == null
                let noPermissionRequired = publicMethod.GetCustomAttribute<NoPermissionRequiredAttribute>()
                where noPermissionRequired == null
                select $"{controllerType.Name}.{publicMethod.Name}").ToList();
            if (!notProtectedActionMethods.Any()) return;
            var errorBuilder = new StringBuilder();
            errorBuilder.AppendLine("Invalid authorization configuration: ");
            foreach (var notProtectedActionMethod in notProtectedActionMethods)
            {
                errorBuilder.AppendLine($"Method {notProtectedActionMethod} is not protected. ");
            }
            throw new ApplicationException(errorBuilder.ToString());
        }
    }
}