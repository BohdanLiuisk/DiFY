using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;

namespace Dify.Core.Application.Users.Queries;

public record GetCurrentUserQuery: IRequest<QueryResponse<UserDto>>;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, QueryResponse<UserDto>>
{
    private readonly IDifyContext _difyContext;
    
    private readonly ICurrentUser _currentUser;
    
    public GetCurrentUserQueryHandler(IDifyContext difyContext, ICurrentUser currentUser)
    {
        _difyContext = difyContext;
        _currentUser = currentUser;
    }
    
    public async Task<QueryResponse<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _difyContext.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Name = u.Name,
                Login = u.Login,
                Email = u.Email,
                AvatarUrl = u.AvatarUrl,
                Online = u.Online,
                CreatedOn = u.CreatedOn
            })
            .FirstOrDefaultAsync(u => u.Id == _currentUser.UserId, cancellationToken);
        user.IsCurrentUser = true;
        return new QueryResponse<UserDto>(user);
    }
}
