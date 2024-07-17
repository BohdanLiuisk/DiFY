using Dify.Entity.Abstract;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlKata;
using SqlKata.Execution;

namespace Dify.Entity.SelectQuery;

public class SelectQueryExecutor(EntityStructureManager structureManager, QueryFactory queryFactory, 
        TableAliasesStorage aliasesStorage) 
    : ISelectQueryExecutor
{
    public async Task<string> ExecuteAsync(SelectQueryConfig selectConfig) {
        var jsonResult = await ExecuteInternal(selectConfig);
        return jsonResult.ToString(Formatting.Indented);
    }
    
    public async Task<SelectQueryResult<T>?> ExecuteAsync<T>(SelectQueryConfig selectConfig) {
        var jsonResult = await ExecuteInternal(selectConfig);
        return jsonResult.ToObject<SelectQueryResult<T>>();
    }

    private async Task<JObject> ExecuteInternal(SelectQueryConfig selectConfig) {
        var rootStructure = await structureManager.FindEntityStructureByName(selectConfig.EntityName);
        var rootTableAlias = aliasesStorage.GetTableAlias(selectConfig.EntityName);
        var rootQuery = CreateRootQuery(selectConfig, rootStructure, rootTableAlias);
        aliasesStorage.Clear();
        return await BuildJsonResult(selectConfig, rootQuery, rootStructure);
    }

    private async Task<JObject> BuildJsonResult(SelectQueryConfig selectConfig, Query rootQuery, 
        EntityStructure rootStructure) {
        var resultBuilder = new JsonResultBuilder()
            .WithSelectQueryConfig(selectConfig)
            .WithRootStructure(rootStructure);
        if (selectConfig.IsPaginated) {
            var paginationResult = await queryFactory.PaginateAsync<dynamic>(rootQuery,
                selectConfig.PaginationConfig!.Page, selectConfig.PaginationConfig!.Page);
            resultBuilder.WithPaginationResult(paginationResult);
            return resultBuilder.BuildPaginatedRowsJson();
        }
        var resultRows = await queryFactory.GetAsync(rootQuery);
        resultBuilder.WithResultRows(resultRows);
        return selectConfig.Limit == 1 ? resultBuilder.BuildSingleRowJson() : resultBuilder.BuildMultipleRowsJson();
    }

    private Query CreateRootQuery(SelectQueryConfig selectConfig, EntityStructure rootStructure, string rootTableAlias) {
        var queryColumns = new SelectColumnBuilder(selectConfig.Columns, rootStructure, rootTableAlias).Build();
        var rootQuery = new Query($"{selectConfig.EntityName} as {rootTableAlias}");
        rootQuery.Select(queryColumns);
        var joinBuilder = new JoinBuilder(rootQuery, aliasesStorage);
        joinBuilder.AppendLeftJoins(selectConfig.Columns, rootTableAlias, rootStructure);
        if (selectConfig.Limit != null && selectConfig.Limit != 0) {
            rootQuery.Limit(selectConfig.Limit.Value);
        }
        return rootQuery;
    }
}
