using DiFY.Modules.UserAccess.Application.Contracts;
using DiFY.Modules.UserAccess.Application.Users.DTOs;

namespace DiFY.Modules.UserAccess.Application.Users.GetAuthenticatedUser
{
    public class GetAuthenticatedUserQuery : QueryBase<UserDto>
    {
        public GetAuthenticatedUserQuery() { }
    }
}