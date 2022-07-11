using DiFY.Modules.Social.Domain.Friendships;
using DiFY.Modules.Social.Domain.Friendships.Interfaces;
using System.Threading.Tasks;

namespace DiFY.Modules.Social.Infrastructure.Domain.Friendships
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly SocialContext _socialContext;

        public FriendshipRepository(SocialContext socialContext) 
        {
            _socialContext = socialContext;
        }

        public async Task AddAsync(Friendship friendship) 
        {
            await _socialContext.Friendships.AddAsync(friendship);
        }
    }
}
