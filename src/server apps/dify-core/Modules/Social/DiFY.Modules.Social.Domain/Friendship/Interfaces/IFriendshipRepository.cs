using System.Threading.Tasks;

namespace DiFY.Modules.Social.Domain.Friendships.Interfaces
{
    public interface IFriendshipRepository
    {
        Task AddAsync(Friendship friendship);
    }
}
