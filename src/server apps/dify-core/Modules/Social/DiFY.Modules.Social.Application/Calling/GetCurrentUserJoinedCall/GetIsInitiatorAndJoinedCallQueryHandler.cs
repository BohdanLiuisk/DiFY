using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.Social.Application.Configuration.Queries;
using DiFY.Modules.Social.Domain.Membership.Abstraction;

namespace DiFY.Modules.Social.Application.Calling.GetCurrentUserJoinedCall;

public class GetIsInitiatorAndJoinedCallQueryHandler : IQueryHandler<GetIsInitiatorAndJoinedCallQuery, bool>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    
    private readonly IMemberContext _memberContext;

    public GetIsInitiatorAndJoinedCallQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IMemberContext memberContext)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _memberContext = memberContext;
    }
    
    public async Task<bool> Handle(GetIsInitiatorAndJoinedCallQuery query, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();
        const string callParticipantCountSelect = "SELECT " +
               "COUNT([CallParticipant].[Id]) AS [CallParticipantCount] " +
               "FROM [social].[CallParticipants] AS [CallParticipant] " +
               "LEFT OUTER JOIN [social].[Calls] AS [Call] " +
               "ON [Call].[Id] = [CallParticipant].[CallId] " +
               "WHERE [CallParticipant].[CallId] = @CallId " +
               "AND [Call].[InitiatorId] = @ParticipantId " +
               "AND [CallParticipant].[ParticipantId] = @ParticipantId " +
               "AND [CallParticipant].[Active] = 1";
        return await connection.QueryFirstAsync<int>(callParticipantCountSelect, new
        {
            query.CallId,
            ParticipantId = _memberContext.MemberId.Value
        }) >= 1;
    }
}
