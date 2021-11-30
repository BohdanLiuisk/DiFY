using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.UserAccess.Domain.UserRegistrations.Interfaces;

namespace DiFY.Modules.UserAccess.Domain.UserRegistrations.Rules
{
    public class UserLoginMustBeUniqueRule : IBusinessRule
    {
        private readonly IUsersCounter _usersCounter;
        private readonly string _login;

        internal UserLoginMustBeUniqueRule(IUsersCounter usersCounter, string login)
        {
            _usersCounter = usersCounter;
            _login = login;
        }

        public bool IsBroken() => _usersCounter.CountUsersWithLogin(_login) > 0;

        public string Message => "User login must be unique.";
    }
}