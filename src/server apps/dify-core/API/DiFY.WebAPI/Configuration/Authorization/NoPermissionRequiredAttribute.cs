using System;

namespace DiFY.WebAPI.Configuration.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class NoPermissionRequiredAttribute : Attribute
    {
        
    }
}