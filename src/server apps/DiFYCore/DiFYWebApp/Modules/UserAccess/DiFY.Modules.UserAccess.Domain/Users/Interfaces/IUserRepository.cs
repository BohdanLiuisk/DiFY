using System.Threading.Tasks;

namespace DiFY.Modules.UserAccess.Domain.Users.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
    }
}