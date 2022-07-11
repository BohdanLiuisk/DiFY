using DiFY.Modules.Social.Domain.FriendshipRequests;
using DiFY.Modules.Social.Domain.FriendshipRequests.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DiFY.Modules.Social.Infrastructure.Domain.FriendshipRequests
{
    public class FriendshipRequestRepository : IFriendshipRequestRepository
    {
        private readonly SocialContext _socialContext;

        public FriendshipRequestRepository(SocialContext socialContext)
        {
            _socialContext = socialContext;
        }

        public async Task AddAsync(FriendshipRequest friendshipRequest)
        {
            await _socialContext.FriendshipRequests.AddAsync(friendshipRequest);
        }

        public async Task<FriendshipRequest> GetByIdAsync(FriendshipRequestId friendshipRequestId)
        {
            return await _socialContext.FriendshipRequests.FirstOrDefaultAsync(
                fr => fr.Id == friendshipRequestId);
        }
    }
}
