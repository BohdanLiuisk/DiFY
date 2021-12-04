using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.UserAccess.Application.Authorization.DTOs;
using DiFY.Modules.UserAccess.Application.Configuration.Queries;

namespace DiFY.Modules.UserAccess.Application.Authorization.GetAuthenticatedUserPermissions
{
    public class GetAuthenticatedUserPermissionsQueryHandler 
        : IQueryHandler<GetAuthenticatedUserPermissionsQuery, List<UserPermissionDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private readonly IExecutionContextAccessor _executionContextAccessor;

        public GetAuthenticatedUserPermissionsQueryHandler(
            ISqlConnectionFactory sqlConnectionFactory,
            IExecutionContextAccessor executionContextAccessor)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task<List<UserPermissionDto>> Handle(GetAuthenticatedUserPermissionsQuery query,
            CancellationToken cancellationToken)
        {
            if (!_executionContextAccessor.IsAvailable)
            {
                return new List<UserPermissionDto>();
            }

            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT [UserPermission].[PermissionCode] AS [Code] " +
                               "FROM [users].[UserPermissions] AS [UserPermission] " +
                               "WHERE [UserPermission].UserId = @UserId";

            var permission = await connection.QueryAsync<UserPermissionDto>(
                sql, new {_executionContextAccessor.UserId });

            return permission.AsList();
        }
    }
}