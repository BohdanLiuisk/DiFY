using Dify.Common.Dto.Friends;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Mapping;

namespace Dify.Core.Application.Friends.Queries;

public record FindPaginatedFriendsQuery(int PageNumber, int PageSize, string SearchValue) 
    : IRequest<QueryResponse<PaginatedList<FoundFriendDto>>>;

internal class FindPaginatedFriendsQueryHandler 
    : IRequestHandler<FindPaginatedFriendsQuery, QueryResponse<PaginatedList<FoundFriendDto>>>
{
    private readonly IDifyContext _difyContext;
    
    public FindPaginatedFriendsQueryHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }
    
    public async Task<QueryResponse<PaginatedList<FoundFriendDto>>> Handle(FindPaginatedFriendsQuery query, 
        CancellationToken cancellationToken)
    {
        var friends = await _difyContext.Users
            .OrderByDescending(u => u.CreatedOn)
            .Where(u => string.IsNullOrEmpty(query.SearchValue) || u.Name.Contains(query.SearchValue))
            .Select(u => new FoundFriendDto(u.Id, u.Name, u.AvatarUrl, new Random().Next(1, 11)))
            .PaginatedListAsync(query.PageNumber, query.PageSize);
        return new QueryResponse<PaginatedList<FoundFriendDto>>(friends);
    }
}
