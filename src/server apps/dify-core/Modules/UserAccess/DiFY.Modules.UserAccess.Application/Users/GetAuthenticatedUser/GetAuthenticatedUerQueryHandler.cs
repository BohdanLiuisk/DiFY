using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.UserAccess.Application.Configuration.Queries;
using DiFY.Modules.UserAccess.Application.Users.DTOs;

namespace DiFY.Modules.UserAccess.Application.Users.GetAuthenticatedUser
{
    internal class GetAuthenticatedUerQueryHandler : IQueryHandler<GetAuthenticatedUserQuery, UserDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private readonly IExecutionContextAccessor _executionContextAccessor;

        public GetAuthenticatedUerQueryHandler(
            ISqlConnectionFactory sqlConnectionFactory,
            IExecutionContextAccessor executionContextAccessor)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task<UserDto> Handle(GetAuthenticatedUserQuery query, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql =
                "SELECT [User].[Id], [User].[IsActive], [User].[Login], [User].[Email], [User].[Name] " +
                "FROM [users].[Users] as [User] " + 
                "WHERE [User].[Id] = @UserId";

            return await connection.QuerySingleAsync<UserDto>(sql, new { _executionContextAccessor.UserId });
        }
    }
}