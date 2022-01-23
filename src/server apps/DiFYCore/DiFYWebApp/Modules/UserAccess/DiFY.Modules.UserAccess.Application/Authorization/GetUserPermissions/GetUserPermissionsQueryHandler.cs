using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.UserAccess.Application.Authorization.DTOs;
using DiFY.Modules.UserAccess.Application.Configuration.Queries;

namespace DiFY.Modules.UserAccess.Application.Authorization.GetUserPermissions
{
    internal class GetUserPermissionsQueryHandler : IQueryHandler<GetUserPermissionsQuery, List<UserPermissionDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetUserPermissionsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<UserPermissionDto>> Handle(GetUserPermissionsQuery query,
            CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT [UserPermission.[PermissionCode] AS [Code] " +
                               "FROM [users].[UserPermissions] AS [UserPermission] " +
                               "WHERE [UserPermission].UserId = @UserId";
            
            var permissions =
                await connection.QueryAsync<UserPermissionDto>(sql, new { query.UserId });

            return permissions.AsList();
        }
    }
}