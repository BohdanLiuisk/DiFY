using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.BuildingBlocks.Application.Queries;
using DiFY.Modules.Social.Application.Configuration.Queries;

namespace DiFY.Modules.Social.Application.Calling.GetAllCalls;

internal class GetAllCallsQueryHandler : IQueryHandler<GetAllCallsQuery, GetAllCallsQueryResult>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllCallsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<GetAllCallsQueryResult> Handle(GetAllCallsQuery query, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();
        var parameters = new DynamicParameters();
        var pageData = PagedQueryManager.GetPageData(query);
        parameters.Add(nameof(PagedQueryManager.Offset), pageData.Offset);
        parameters.Add(nameof(PagedQueryManager.Next), pageData.Next);
        var sorting = SqlSortBuilder.BuildSorting(query.SortOptions, "[Call]");
        if (string.IsNullOrWhiteSpace(sorting))
        {
            sorting = "[Call].[StartDate] DESC";
        }
        var selectCall = "SELECT " +
                  $"[Call].[Id] AS [{nameof(GetAllCallsDto.Id)}], " +
                  $"[Call].[Name] AS [{nameof(GetAllCallsDto.Name)}], " +
                  $"[Call].[Active] AS [{nameof(GetAllCallsDto.Active)}], " +
                  $"[Call].[StartDate] AS [{nameof(GetAllCallsDto.StartDate)}], " +
                  $"[Call].[EndDate] AS [{nameof(GetAllCallsDto.EndDate)}], " +
                  $"[Call].[ActiveParticipants] AS [{nameof(GetAllCallsDto.ActiveParticipants)}], " +
                  $"[Call].[TotalParticipants] AS [{nameof(GetAllCallsDto.TotalParticipants)}]" +
                  $"FROM [social].[v_Calls] AS [Call] " +
                  $"ORDER BY {sorting}";
        var selectTotalCount = "SELECT COUNT([Call].[Id]) FROM [social].[v_Calls] AS [Call]";
        return new GetAllCallsQueryResult
        {
            Calls = await connection.QueryAsync<GetAllCallsDto>(
                PagedQueryManager.AppendPageStatement(selectCall), parameters),
            TotalCount = await connection.QuerySingleAsync<int>(selectTotalCount)
        };
    }
}
