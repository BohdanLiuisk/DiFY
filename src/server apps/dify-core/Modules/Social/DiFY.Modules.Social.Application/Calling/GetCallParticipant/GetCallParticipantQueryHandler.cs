using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.Social.Application.Configuration.Queries;

namespace DiFY.Modules.Social.Application.Calling.GetCallParticipant;

public class GetCallParticipantQueryHandler : IQueryHandler<GetCallParticipantQuery, CallParticipantDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    
    public GetCallParticipantQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<CallParticipantDto> Handle(
        GetCallParticipantQuery request, CancellationToken cancellationToken) 
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();
        const string participantSelect = "SELECT " +
              $"[CallParticipant].[ParticipantId] AS [{nameof(CallParticipantDto.Id)}], " +
              $"[CallParticipant].[JoinOn] AS [{nameof(CallParticipantDto.JoinOn)}], " +
              $"[CallParticipant].[Active] AS [{nameof(CallParticipantDto.Active)}], " +
              $"[Member].[Name] AS [{nameof(CallParticipantDto.Name)}] " +
              "FROM [social].[CallParticipants] as [CallParticipant] " +
              "LEFT OUTER JOIN [social].[Members] as [Member] " +
              "ON [Member].[Id] = [CallParticipant].[ParticipantId] " +
              $"WHERE [CallParticipant].[ParticipantId] = @ParticipantId";
        return await connection.QueryFirstOrDefaultAsync<CallParticipantDto>(
            participantSelect, new { request.ParticipantId });
    }
}