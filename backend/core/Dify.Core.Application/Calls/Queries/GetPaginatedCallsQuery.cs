using Dify.Common.Dto.CallHistory;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Mapping;

namespace Dify.Core.Application.Calls.Queries;

public record GetPaginatedCallsQuery(int PageNumber, int PageSize) : IRequest<QueryResponse<PaginatedList<CallHistoryDto>>>;

public class GetPaginatedCallsQueryHandler 
    : IRequestHandler<GetPaginatedCallsQuery, QueryResponse<PaginatedList<CallHistoryDto>>>
{
    private readonly IDifyContext _difyContext;
    
    private readonly ICurrentUser _currentUser;
    
    public GetPaginatedCallsQueryHandler(IDifyContext difyContext, ICurrentUser currentUser)
    {
        _difyContext = difyContext;
        _currentUser = currentUser;
    }
    
    public async Task<QueryResponse<PaginatedList<CallHistoryDto>>> Handle(GetPaginatedCallsQuery query, 
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.UserId;
        var calls = await _difyContext.Calls
            .OrderByDescending(u => u.CreatedOn)
            .Where(c => c.Participants.Any(p => p.ParticipantId == currentUserId))
            .Select(c => new CallHistoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Active = c.Active,
                InitiatorId = c.CreatedById,
                StartDate = c.CreatedOn,
                Direction = (int)c.Participants
                    .FirstOrDefault(p => p.ParticipantId == currentUserId).Direction,
                Participants = c.Participants
                    .Where(p => p.ParticipantId != currentUserId)
                    .Select(p => new CallParticipantHistoryDto
                    {
                        Id = p.Participant.Id,
                        CallParticipantId = p.Id,
                        Name = p.Participant.Name,
                        AvatarUrl = p.Participant.AvatarUrl,
                        IsOnline = p.Participant.Online,
                        CallDirection = (int)p.Direction
                    })
            })
            .PaginatedListAsync(query.PageNumber, query.PageSize);
        return new QueryResponse<PaginatedList<CallHistoryDto>>(calls);
    }
}
