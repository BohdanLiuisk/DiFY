using System.Threading.Tasks;

namespace DiFY.Modules.Social.Domain.FriendshipRequest.Interfaces
{
    public interface IFriendshipRequestRepository
    {
        Task AddAsync(FriendshipRequest friendshipRequest);

        Task<FriendshipRequest> GetByIdAsync(FriendshipRequestId friendshipRequestId);
    }
}
