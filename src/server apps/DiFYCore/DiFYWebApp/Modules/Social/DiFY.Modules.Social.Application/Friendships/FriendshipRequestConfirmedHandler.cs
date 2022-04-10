using DiFY.Modules.Social.Domain.FriendshipRequests.Events;
using DiFY.Modules.Social.Domain.FriendshipRequests.Interfaces;
using DiFY.Modules.Social.Domain.Friendships.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DiFY.Modules.Social.Application.Friendships
{
    public class FriendshipRequestConfirmedHandler : INotificationHandler<FriendshipRequestConfirmedDomainEvent>
    {
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;

        private readonly IFriendshipRepository _friendshipRepository;

        public FriendshipRequestConfirmedHandler(
            IFriendshipRequestRepository friendshipRequestRepository, 
            IFriendshipRepository friendshipRepository)
        {
            _friendshipRepository = friendshipRepository;

            _friendshipRequestRepository = friendshipRequestRepository;
        }

        public async Task Handle(FriendshipRequestConfirmedDomainEvent @event, CancellationToken cancellationToken)
        {
            var friendshipRequest = await _friendshipRequestRepository.GetByIdAsync(@event.FriendshipRequestId);

            var friendship = friendshipRequest.CreateFriendship();

            await _friendshipRepository.AddAsync(friendship);
        }
    }
}
