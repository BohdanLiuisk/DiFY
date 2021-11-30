using System.Threading.Tasks;

namespace DiFY.Modules.UserAccess.Domain.UserRegistrations.Interfaces
{
    public interface IUserRegistrationRepository
    {
        Task AddAsync(UserRegistration userRegistration);

        Task<UserRegistration> GetByIdAsync(UserRegistrationId userRegistrationId);
    }
}