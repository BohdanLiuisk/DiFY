using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Exceptions;

namespace Dify.Core.Application.Users.Queries;

public record GetUserByIdQuery(int Id) : IRequest<QueryResponse<UserDto>>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, QueryResponse<UserDto>>
{
    private readonly IDifyContext _difyContext;
    
    private readonly IMapper _mapper;
    
    private readonly ICurrentUser _currentUser;

    public GetUserByIdQueryHandler(IDifyContext difyContext, IMapper mapper, ICurrentUser currentUser)
    {
        _difyContext = difyContext;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    
    public async Task<QueryResponse<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _difyContext.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(u => u.Id == query.Id, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User with id {query.Id} was not found.");
        }
        var mappedUser = _mapper.Map<UserDto>(user);
        mappedUser.IsCurrentUser = user.Id == _currentUser.UserId;
        return new QueryResponse<UserDto>(mappedUser);
    }
}
