using DiFY.Modules.Social.Application.Configuration.Commands;
using DiFY.Modules.Social.Domain.FriendshipRequests;
using DiFY.Modules.Social.Domain.FriendshipRequests.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiFY.Modules.Social.Application.FriendshipRequests.CreateFriendshipRequest
{
    internal class CreateFriendshipRequestCommandHandler : ICommandHandler<CreateFriendshipRequestCommand, Guid>
    {
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;

        private readonly IFriendshipRequestService _friendshipRequestService;

        public CreateFriendshipRequestCommandHandler(
            IFriendshipRequestRepository friendshipRequestRepository,
            IFriendshipRequestService friendshipRequestService)
        {
            _friendshipRequestRepository = friendshipRequestRepository;

            _friendshipRequestService = friendshipRequestService;
        }

        public async Task<Guid> Handle(CreateFriendshipRequestCommand command, CancellationToken cancellationToken)
        {
            var friendshipRequest = FriendshipRequest.CreateNewFriendshipRequest(
                new RequesterId(command.RequesterId),
                new AddresseeId(command.AddresseeId),
                command.CreateDate,
                _friendshipRequestService);

            await _friendshipRequestRepository.AddAsync(friendshipRequest);

            return friendshipRequest.Id.Value;
        }
    }
}
