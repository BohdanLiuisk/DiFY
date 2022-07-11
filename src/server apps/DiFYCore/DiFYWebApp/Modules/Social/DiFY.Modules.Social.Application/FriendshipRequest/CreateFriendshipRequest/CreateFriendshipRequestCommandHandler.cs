using DiFY.Modules.Social.Application.Configuration.Commands;
using DiFY.Modules.Social.Domain.FriendshipRequests.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application.Data;

namespace DiFY.Modules.Social.Application.FriendshipRequests.CreateFriendshipRequest
{
    using DiFY.Modules.Social.Domain.FriendshipRequests;

    internal class CreateFriendshipRequestCommandHandler : ICommandHandler<CreateFriendshipRequestCommand, Guid>
    {
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;
        
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        
        public CreateFriendshipRequestCommandHandler(
            IFriendshipRequestRepository friendshipRequestRepository, ISqlConnectionFactory sqlConnectionFactory)
        {
            _friendshipRequestRepository = friendshipRequestRepository;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Guid> Handle(CreateFriendshipRequestCommand command, CancellationToken cancellationToken)
        {
            var friendshipRequest = FriendshipRequest.CreateNewFriendshipRequest(
                new RequesterId(command.RequesterId),
                new AddresseeId(command.AddresseeId),
                command.CreateDate,
                CountActiveFriendshipRequests);
            await _friendshipRequestRepository.AddAsync(friendshipRequest);
            return friendshipRequest.Id.Value;
        }
        
        private int CountActiveFriendshipRequests(Guid firstParticipant, Guid secondParticipant)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            const string query = "SELECT COUNT(*)" + 
                                 "FROM [social].[FriendshipRequest] as [FriendshipRequest] " +
                                 "WHERE [FriendshipRequest].[AddresseeId] = @AddresseeId " +
                                 "AND [FriendshipRequest].[RequesterId] = @RequesterId";
            return connection.QuerySingle<int>(query, new
            {
                AddresseeId = secondParticipant,
                RequesterId = firstParticipant
            });
        }
    }
}
