using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Mapping;

namespace Dify.Core.Application.Users.Queries;

public record GetPaginatedUsersQuery(
    int PageNumber, int PageSize
) : IRequest<QueryResponse<PaginatedList<UserDto>>>;

public class GetPaginatedUsersQueryHandler : IRequestHandler<GetPaginatedUsersQuery, QueryResponse<PaginatedList<UserDto>>>
{
    private readonly IDifyContext _difyContext;
    
    public GetPaginatedUsersQueryHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }

    public async Task<QueryResponse<PaginatedList<UserDto>>> Handle(GetPaginatedUsersQuery query, 
        CancellationToken cancellationToken)
    {
        var users = await _difyContext.Users
            .OrderBy(u => u.Name)
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
            .PaginatedListAsync(query.PageNumber, query.PageSize);
        return new QueryResponse<PaginatedList<UserDto>>(users);
    }
}
