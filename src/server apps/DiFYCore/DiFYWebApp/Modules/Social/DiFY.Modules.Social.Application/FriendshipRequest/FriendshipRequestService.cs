using Dapper;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.Social.Domain.FriendshipRequests.Interfaces;
using System;

namespace DiFY.Modules.Social.Application.FriendshipRequest
{
    public class FriendshipRequestService : IFriendshipRequestService
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public FriendshipRequestService(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public int GetFriendshipRequestsCount(Guid firstParticipant, Guid secondParticipant)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            var sqlQuery = GetFriendshipRequestsCountSql();

            return connection.QuerySingle<int>(sqlQuery, new
            {
                RequesterId = firstParticipant,
                AddresseeId = secondParticipant,
            });
        }

        private static string GetFriendshipRequestsCountSql() =>
                "SELECT " +
                "COUNT(*) " +
                "FROM [social].[FriendshipRequest] as [FriendshipRequest] " +
                "WHERE [FriendshipRequest].[AddresseeId] = @AddresseeId " +
                "AND [FriendshipRequest].[RequesterId] = @RequesterId ";
    }
}
