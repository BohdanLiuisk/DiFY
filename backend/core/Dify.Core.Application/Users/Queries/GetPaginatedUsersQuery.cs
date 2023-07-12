using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Mapping;
using MediatR;

namespace Dify.Core.Application.Users.Queries;

public record GetPaginatedUsersQuery(
    int PageNumber, int PageSize
) : IRequest<QueryResponse<PaginatedList<UserDto>>>;

public class GetPaginatedUsersQueryHandler : IRequestHandler<GetPaginatedUsersQuery, QueryResponse<PaginatedList<UserDto>>>
{
    private readonly IDifyContext _difyContext;
    
    private readonly IMapper _mapper;
    
    public GetPaginatedUsersQueryHandler(IDifyContext difyContext, IMapper mapper)
    {
        _difyContext = difyContext;
        _mapper = mapper;
    }

    public async Task<QueryResponse<PaginatedList<UserDto>>> Handle(GetPaginatedUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await _difyContext.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(query.PageNumber, query.PageSize);
        var response = new QueryResponse<PaginatedList<UserDto>>();
        response.SetBody(_mapper.Map<PaginatedList<UserDto>>(users));
        return response;
    }
}
