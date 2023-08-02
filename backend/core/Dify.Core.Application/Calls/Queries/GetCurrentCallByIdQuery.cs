using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Exceptions;

namespace Dify.Core.Application.Calls.Queries;

public record GetCurrentCallByIdQuery(Guid CallId): IRequest<QueryResponse<CurrentCallDto>>;

public class GetCurrentCallByIdQueryHandler : IRequestHandler<GetCurrentCallByIdQuery, QueryResponse<CurrentCallDto>>
{
    private readonly IDifyContext _difyContext;
    
    private readonly IMapper _mapper;

    public GetCurrentCallByIdQueryHandler(IDifyContext difyContext, IMapper mapper)
    {
        _difyContext = difyContext;
        _mapper = mapper;
    }

    public async Task<QueryResponse<CurrentCallDto>> Handle(GetCurrentCallByIdQuery query, 
        CancellationToken cancellationToken)
    {
        var callWithParticipants = await _difyContext.Calls
            .Include(c => c.Participants)
            .ThenInclude(p => p.Participant)
            .Select(c => new
            {
                Call = new CallDto
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
                },
                Participants = c.Participants.Select(p => new CallParticipantDto
                {
                    Id = p.Id,
                    ParticipantId = p.ParticipantId,
                    Name = p.Participant.Name,
                    StreamId = p.StreamId,
                    PeerId = p.PeerId,
                    ConnectionId = p.ConnectionId,
                    Active = p.Active,
                    JoinedAt = p.JoinedAt
                }).ToList()
            })
            .FirstOrDefaultAsync(c => c.Call.Id == query.CallId, cancellationToken);
        if (callWithParticipants is null)
        {
            throw new NotFoundException($"Call with id {query.CallId} not found.");
        }
        var currentCallDto = new CurrentCallDto(callWithParticipants.Call, callWithParticipants.Participants);
        return new QueryResponse<CurrentCallDto>(currentCallDto);
    }
}
