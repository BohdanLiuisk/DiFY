using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.UserAccess.Application.Configuration.Commands;
using DiFY.Modules.UserAccess.Application.Contracts;

namespace DiFY.Modules.UserAccess.Application.Authentication.Authenticate
{
    internal class AuthenticationCommandHandler : ICommandHandler<AuthenticateCommand, AuthenticationResult>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        internal AuthenticationCommandHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        
        public async Task<AuthenticationResult> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql =
                "SELECT [User].[Id], [User].[Login], [User].[Name], [User.Email], [User].[IsActive], [User].Password " +
                "FROM [users].[Users] AS [User] " +
                "WHERE [User].[Login] = @Login";

            var user = await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new  { request.Login });

            if (user == null)
            {
                return new AuthenticationResult("Incorrect login.");
            }

            if (!user.IsActive)
            {
                return new AuthenticationResult("User id not active.");
            }

            if (!PasswordHashManager.VerifyHashedPassword(user.Password, request.Password))
            {
                return new AuthenticationResult("Incorrect password.");
            }

            user.Claims = new List<Claim>()
            {
                new(CustomClaimTypes.Name, user.Name),
                new(CustomClaimTypes.Email, user.Email)
            };

            return new AuthenticationResult(user);
        }
    }
}