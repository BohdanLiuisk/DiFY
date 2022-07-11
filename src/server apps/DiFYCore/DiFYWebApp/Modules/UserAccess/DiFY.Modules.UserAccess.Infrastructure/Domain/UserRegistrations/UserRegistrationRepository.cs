using System.Threading.Tasks;
using DiFY.Modules.UserAccess.Domain.UserRegistrations;
using DiFY.Modules.UserAccess.Domain.UserRegistrations.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiFY.Modules.UserAccess.Infrastructure.Domain.UserRegistrations
{
    public class UserRegistrationRepository : IUserRegistrationRepository
    {
        private readonly UserAccessContext _userAccessContext;

        public UserRegistrationRepository(UserAccessContext userAccessContext)
        {
            _userAccessContext = userAccessContext;
        }
        
        public async Task AddAsync(UserRegistration userRegistration)
        {
            await _userAccessContext.UserRegistrations.AddAsync(userRegistration);
        }

        public async Task<UserRegistration> GetByIdAsync(UserRegistrationId userRegistrationId)
        {
            return await _userAccessContext.UserRegistrations.FirstOrDefaultAsync(
                ur => ur.Id == userRegistrationId);
        }
    }
}