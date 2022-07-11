using System.Threading.Tasks;
using DiFY.Modules.UserAccess.Domain.Users;
using DiFY.Modules.UserAccess.Domain.Users.Interfaces;

namespace DiFY.Modules.UserAccess.Infrastructure.Domain.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly UserAccessContext _userAccessContext;

        public UserRepository(UserAccessContext userAccessContext)
        {
            _userAccessContext = userAccessContext;
        }
        
        public async Task AddAsync(User user)
        {
            await _userAccessContext.Users.AddAsync(user);
        }
    }
}