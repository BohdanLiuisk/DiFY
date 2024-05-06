using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;

namespace Dify.Core.Application.Calls.Queries;

public record SearchParticipantsForCallQuery(
    string SearchValue
): IRequest<QueryResponse<ICollection<ParticipantForCallDto>>>;

public class SearchParticipantsForCallQueryHandler : IRequestHandler<SearchParticipantsForCallQuery, 
    QueryResponse<ICollection<ParticipantForCallDto>>>
{
    private readonly IDifyContext _difyContext;
    public SearchParticipantsForCallQueryHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }

    public async Task<QueryResponse<ICollection<ParticipantForCallDto>>> Handle(SearchParticipantsForCallQuery query, 
        CancellationToken cancellationToken)
    {
        var participants = await _difyContext.Users
            .Take(50)
            .Where(u => string.IsNullOrEmpty(query.SearchValue) || u.Name.Contains(query.SearchValue))
            .Select(u => new ParticipantForCallDto 
            {
                Id = u.Id,
                Username = u.Login,
                Name = u.Name,
                IsOnline = u.Online,
                AvatarUrl = u.AvatarUrl
            })
            .ToListAsync(cancellationToken);
        return new QueryResponse<ICollection<ParticipantForCallDto>>(participants);
    }
}
