using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Mapping;

namespace Dify.Core.Application.Calls.Queries;

public record GetPaginatedCallsQuery(int PageNumber, int PageSize) : IRequest<QueryResponse<PaginatedList<CallDto>>>;

public class GetPaginatedCallsQueryHandler : IRequestHandler<GetPaginatedCallsQuery, QueryResponse<PaginatedList<CallDto>>>
{
    private readonly IDifyContext _difyContext;
    
    private readonly ICurrentUser _currentUser;
    
    public GetPaginatedCallsQueryHandler(IDifyContext difyContext, ICurrentUser currentUser)
    {
        _difyContext = difyContext;
        _currentUser = currentUser;
    }
    
    public async Task<QueryResponse<PaginatedList<CallDto>>> Handle(GetPaginatedCallsQuery query, 
        CancellationToken cancellationToken)
    {
        var calls = await _difyContext.Calls
            .OrderByDescending(u => u.CreatedOn)
            .Where(c => c.Participants.Any(p => p.ParticipantId == _currentUser.UserId))
            .Select(c => new CallDto
            {
                Id = c.Id,
                Name = c.Name,
                Active = c.Active,
                InitiatorId = c.CreatedById,
                StartDate = c.CreatedOn,
                EndDate = c.EndDate,
                Duration = c.Duration,
                TotalParticipants = c.Participants.Count,
                ActiveParticipants = c.Participants.Count(p => p.Active)
            })
            .PaginatedListAsync(query.PageNumber, query.PageSize);
        return new QueryResponse<PaginatedList<CallDto>>(calls);
    }
}
