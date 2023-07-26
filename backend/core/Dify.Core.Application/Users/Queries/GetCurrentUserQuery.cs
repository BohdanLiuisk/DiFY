using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;

namespace Dify.Core.Application.Users.Queries;

public record GetCurrentUserQuery: IRequest<QueryResponse<UserDto>>;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, QueryResponse<UserDto>>
{
    private readonly IDifyContext _difyContext;
    
    private readonly IMapper _mapper;
    
    private readonly ICurrentUser _currentUser;
    
    public GetCurrentUserQueryHandler(IDifyContext difyContext, IMapper mapper, ICurrentUser currentUser)
    {
        _difyContext = difyContext;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    
    public async Task<QueryResponse<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _difyContext.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(u => u.Id == _currentUser.UserId, cancellationToken);
        var mappedUser = _mapper.Map<UserDto>(user);
        mappedUser.IsCurrentUser = true;
        return new QueryResponse<UserDto>(mappedUser);
    }
}
