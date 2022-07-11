namespace DiFY.Modules.UserAccess.Domain.UserRegistrations.Interfaces
{
    public interface IUsersCounter
    {
        int CountUsersWithLogin(string login);
    }
}