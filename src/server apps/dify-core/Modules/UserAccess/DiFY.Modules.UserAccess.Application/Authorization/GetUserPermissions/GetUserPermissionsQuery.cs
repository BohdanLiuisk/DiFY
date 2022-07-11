using System;
using System.Collections.Generic;
using DiFY.Modules.UserAccess.Application.Authorization.DTOs;
using DiFY.Modules.UserAccess.Application.Contracts;

namespace DiFY.Modules.UserAccess.Application.Authorization.GetUserPermissions
{
    public class GetUserPermissionsQuery : QueryBase<List<UserPermissionDto>>
    {
        public GetUserPermissionsQuery(Guid userId)
        {
            UserId = userId;
        }
        
        public Guid UserId { get; set; }
    }
}