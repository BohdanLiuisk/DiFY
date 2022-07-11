using DiFY.Modules.Social.Application.Configuration.Commands;
using DiFY.Modules.Social.Domain.FriendshipRequests;
using DiFY.Modules.Social.Domain.FriendshipRequests.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiFY.Modules.Social.Application.FriendshipRequest.RejectFriendshipRequest
{
    internal class RejectFriendshipRequestCommandHandler : ICommandHandler<RejectFriendshipRequestCommand>
    {
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;

        public RejectFriendshipRequestCommandHandler(IFriendshipRequestRepository friendshipRequestRepository)
        {
            _friendshipRequestRepository = friendshipRequestRepository;
        }

        public async Task<Unit> Handle(RejectFriendshipRequestCommand command, CancellationToken cancellationToken)
        {
            var friendshipRequest = await _friendshipRequestRepository
                .GetByIdAsync(new FriendshipRequestId(command.FriendshipRequestId));
            friendshipRequest.Reject(DateTime.UtcNow);
            return Unit.Value;
        }
    }
}
