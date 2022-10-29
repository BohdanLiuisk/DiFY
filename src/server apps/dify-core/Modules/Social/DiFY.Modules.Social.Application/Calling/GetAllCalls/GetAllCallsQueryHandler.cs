using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.BuildingBlocks.Application.Queries;
using DiFY.Modules.Social.Application.Configuration.Queries;

namespace DiFY.Modules.Social.Application.Calling.GetAllCalls;

internal class GetAllCallsQueryHandler : IQueryHandler<GetAllCallsQuery, IEnumerable<CallDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllCallsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<IEnumerable<CallDto>> Handle(GetAllCallsQuery query, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();
        var parameters = new DynamicParameters();
        var pageData = PagedQueryManager.GetPageData(query);
        parameters.Add(nameof(PagedQueryManager.Offset), pageData.Offset);
        parameters.Add(nameof(PagedQueryManager.Next), pageData.Next);
        var sql = "SELECT " +
                  $"[Call].[Id] AS [{nameof(CallDto.Id)}], " +
                  $"[Call].[Name] AS [{nameof(CallDto.Name)}], " +
                  $"[Call].[Active] AS [{nameof(CallDto.Active)}], " +
                  $"[Call].[StartDate] AS [{nameof(CallDto.StartDate)}], " +
                  $"[Call].[EndDate] AS [{nameof(CallDto.EndDate)}], " +
                  $"[Call].[ActiveParticipants] AS [{nameof(CallDto.ActiveParticipants)}], " +
                  $"[Call].[TotalParticipants] AS [{nameof(CallDto.TotalParticipants)}]" +
                  $"FROM [social].[v_Calls] AS [Call] " +
                  $"ORDER BY [Call].[StartDate] DESC";
        return await connection.QueryAsync<CallDto>(PagedQueryManager.AppendPageStatement(sql), parameters);
    }
}
