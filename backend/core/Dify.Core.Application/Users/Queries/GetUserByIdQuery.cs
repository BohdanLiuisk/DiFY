using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Exceptions;

namespace Dify.Core.Application.Users.Queries;

public record GetUserByIdQuery(int Id) : IRequest<QueryResponse<UserDto>>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, QueryResponse<UserDto>>
{
    private readonly IDifyContext _difyContext;
    
    private readonly ICurrentUser _currentUser;

    public GetUserByIdQueryHandler(IDifyContext difyContext, ICurrentUser currentUser)
    {
        _difyContext = difyContext;
        _currentUser = currentUser;
    }
    
    public async Task<QueryResponse<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
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
            .FirstOrDefaultAsync(u => u.Id == query.Id, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User with id {query.Id} was not found.");
        }
        user.IsCurrentUser = user.Id == _currentUser.UserId;
        return new QueryResponse<UserDto>(user);
    }
}
