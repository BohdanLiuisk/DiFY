using DiFY.Modules.Social.Application.Configuration.Commands;
using DiFY.Modules.Social.Domain.FriendshipRequests;
using DiFY.Modules.Social.Domain.FriendshipRequests.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiFY.Modules.Social.Application.FriendshipRequest.ConfirmFriendshipRequest
{
    internal class ConfirmFriendshipRequestCommandHandler : ICommandHandler<ConfirmFriendshipRequestCommand>
    {
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;

        public ConfirmFriendshipRequestCommandHandler(IFriendshipRequestRepository friendshipRequestRepository)
        {
            _friendshipRequestRepository = friendshipRequestRepository;
        }

        public async Task<Unit> Handle(ConfirmFriendshipRequestCommand command, CancellationToken cancellationToken)
        {
            var friendshipRequest = await _friendshipRequestRepository.GetByIdAsync(
                new FriendshipRequestId(command.FriendshipRequestId));
            friendshipRequest.Confirm(DateTime.UtcNow);
            return Unit.Value;
        }
    }
}
