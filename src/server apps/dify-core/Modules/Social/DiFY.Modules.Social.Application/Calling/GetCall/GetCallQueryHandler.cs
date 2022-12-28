using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.Social.Application.Configuration.Queries;

namespace DiFY.Modules.Social.Application.Calling.GetCall;

public class GetCallQueryHandler : IQueryHandler<GetCallQuery, GetCallQueryResult>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    
    public GetCallQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<GetCallQueryResult> Handle(GetCallQuery request, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();
        const string callSelect = "SELECT " +
                                  $"[Call].[Id] AS [{nameof(GetCallDto.Id)}], " +
                                  $"[Call].[Name] AS [{nameof(GetCallDto.Name)}], " +
                                  $"[Call].[StartDate] AS [{nameof(GetCallDto.StartDate)}], " +
                                  $"[Call].[ActiveParticipants] AS [{nameof(GetCallDto.ActiveParticipants)}], " +
                                  $"[Call].[TotalParticipants] AS [{nameof(GetCallDto.TotalParticipants)}] " +
                                  "FROM [social].[v_Calls] AS [Call] " +
                                  "WHERE [Call].[Id] = @CallId";
        const string participantsSelect = "SELECT " +
                                 $"[CallParticipant].[ParticipantId] AS [{nameof(CallParticipantDto.Id)}], " +
                                 $"[CallParticipant].[JoinOn] AS [{nameof(CallParticipantDto.JoinOn)}], " +
                                 $"[CallParticipant].[Active] AS [{nameof(CallParticipantDto.Active)}], " +
                                 $"[CallParticipant].[StreamId] AS [{nameof(CallParticipantDto.StreamId)}], " +
                                 $"[CallParticipant].[PeerId] AS [{nameof(CallParticipantDto.PeerId)}], " +
                                 $"[CallParticipant].[ConnectionId] AS [{nameof(CallParticipantDto.ConnectionId)}], " +
                                 $"[Member].[Name] AS [{nameof(CallParticipantDto.Name)}] " +
                                 "FROM [social].[CallParticipants] as [CallParticipant] " +
                                 "LEFT OUTER JOIN [social].[Members] as [Member] " +
                                 "ON [Member].[Id] = [CallParticipant].[ParticipantId] " +
                                 $"WHERE [CallParticipant].[CallId] = @CallId";
        return new GetCallQueryResult
        {
            Call = await connection.QueryFirstOrDefaultAsync<GetCallDto>(
                callSelect, new { request.CallId }),
            Participants = await connection.QueryAsync<CallParticipantDto>(
                participantsSelect, new { request.CallId })
        };
    }
}
