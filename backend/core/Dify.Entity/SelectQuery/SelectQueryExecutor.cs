using Dify.Entity.Abstract;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlKata;
using SqlKata.Execution;

namespace Dify.Entity.SelectQuery;

public class SelectQueryExecutor(EntityStructureManager structureManager, QueryFactory queryFactory, 
        TableJoinsStorage joinsStorage) 
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
        var rootTableAlias = joinsStorage.GetTableAlias(selectConfig.EntityName);
        var rootQuery = CreateRootQuery(selectConfig, rootStructure, rootTableAlias);
        joinsStorage.Clear();
        return await BuildJsonResult(selectConfig, rootQuery, rootStructure);
    }

    private async Task<JObject> BuildJsonResult(SelectQueryConfig selectConfig, Query rootQuery, 
        EntityStructure rootStructure) {
        var resultBuilder = new SelectResultBuilder(selectConfig, rootStructure);
        if (selectConfig.IsPaginated) {
            var paginationResult = await queryFactory.PaginateAsync<dynamic>(rootQuery,
                selectConfig.PaginationConfig!.Page, selectConfig.PaginationConfig!.PerPage);
            return resultBuilder.BuildPaginatedRows(paginationResult);
        }
        var resultRows = await queryFactory.GetAsync(rootQuery);
        if (selectConfig.Limit == 1) {
            var firstRow = resultRows.FirstOrDefault() as IDictionary<string, object>;
            return resultBuilder.BuildSingleRowJson(firstRow);
        }
        return resultBuilder.BuildMultipleRowsJson(resultRows);
    }

    private Query CreateRootQuery(SelectQueryConfig selectConfig, EntityStructure rootStructure, string rootTableAlias) {
        var selectBuilder = new SelectColumnBuilder(selectConfig.Expressions, rootStructure, rootTableAlias);
        var queryColumns = selectBuilder.BuildAliases();
        var rootQuery = new Query($"{selectConfig.EntityName} as {rootTableAlias}");
        rootQuery.Select(queryColumns);
        var joinBuilder = new JoinBuilder(rootQuery, joinsStorage);
        joinBuilder.AppendLeftJoins(selectConfig.Expressions, rootTableAlias, rootStructure);
        if (selectConfig.Limit != null && selectConfig.Limit != 0) {
            rootQuery.Limit(selectConfig.Limit.Value);
        }
        return rootQuery;
    }
}
