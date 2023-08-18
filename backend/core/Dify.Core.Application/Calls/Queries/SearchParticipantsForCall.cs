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
    
    private readonly IMapper _mapper;

    public SearchParticipantsForCallQueryHandler(IDifyContext difyContext, IMapper mapper)
    {
        _difyContext = difyContext;
        _mapper = mapper;
    }

    public async Task<QueryResponse<ICollection<ParticipantForCallDto>>> Handle(SearchParticipantsForCallQuery query, 
        CancellationToken cancellationToken)
    {
        var participants = await _difyContext.Users
            .Take(50)
            .Where(u => string.IsNullOrEmpty(query.SearchValue) || u.Name.Contains(query.SearchValue))
            .ProjectTo<ParticipantForCallDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return new QueryResponse<ICollection<ParticipantForCallDto>>(participants);
    }
}
