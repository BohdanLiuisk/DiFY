using Dapper;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.UserAccess.Domain.UserRegistrations.Interfaces;

namespace DiFY.Modules.UserAccess.Application.UserRegistrations.Services
{
    public class UsersCounter : IUsersCounter
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public UsersCounter(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public int CountUsersWithLogin(string login)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            
            const string sql = "SELECT " +
                               "COUNT(*) " +
                               "FROM [users].[Users] as [User] " +
                               "WHERE [User].[Login] = @Login";

            return connection.QuerySingle<int>(sql, new { login });
        }
    }
}