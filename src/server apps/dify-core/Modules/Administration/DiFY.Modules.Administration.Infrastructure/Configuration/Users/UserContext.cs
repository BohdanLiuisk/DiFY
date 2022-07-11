using DiFY.BuildingBlocks.Application;
using DiFY.Modules.Administration.Domain.Users;

namespace DiFY.Modules.Administration.Infrastructure.Configuration.Users
{
    public class UserContext : IUserContext
    {
        private readonly IExecutionContextAccessor _executionContextAccessor;

        public UserContext(IExecutionContextAccessor executionContextAccessor)
        {
            _executionContextAccessor = executionContextAccessor;
        }

        public UserId UserId => new (_executionContextAccessor.UserId);
    }
}