using Dify.Common.Dto.Call;
using Dify.Common.Models;
using Dify.Core.Application.Common;

namespace Dify.Core.Application.Calls.Queries;

public record GetCanJoinCallQuery(Guid CallId): IRequest<QueryResponse<CanJoinCallDto>>;

public class GetCanJoinCallQueryHandler : IRequestHandler<GetCanJoinCallQuery, QueryResponse<CanJoinCallDto>>
{
    private readonly IDifyContext _difyContext;
    
    private readonly ICurrentUser _currentUser;
    
    public GetCanJoinCallQueryHandler(IDifyContext difyContext, ICurrentUser currentUser)
    {
        _difyContext = difyContext;
        _currentUser = currentUser;
    }
    
    public async Task<QueryResponse<CanJoinCallDto>> Handle(GetCanJoinCallQuery query, CancellationToken cancellationToken)
    {
        var call = await _difyContext.Calls
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == query.CallId, cancellationToken);
        var success = call.GetCanJoin(_currentUser.UserId);
        var errorMessage = !success ? "Call is already over." : string.Empty;
        return new QueryResponse<CanJoinCallDto>(new CanJoinCallDto(success, errorMessage));
    }
}
