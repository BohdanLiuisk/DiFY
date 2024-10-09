using Dify.Entity.Abstract;
using Dify.Entity.SelectQuery.Enums;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlKata;
using SqlKata.Execution;
using SqlKata.Compilers;

namespace Dify.Entity.SelectQuery;

public class SelectQueryExecutor(EntityStructureManager structureManager, QueryFactory queryFactory, 
        AliasStorage aliasStorage) 
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
        var rootQuery = CreateRootQuery(selectConfig, rootStructure);
        aliasStorage.Clear();
        return await BuildJsonResult(selectConfig, rootQuery, rootStructure);
    }

    private async Task<JObject> BuildJsonResult(SelectQueryConfig selectConfig, Query rootQuery, 
        EntityStructure rootStructure) {
        var resultBuilder = new SelectResultBuilder(selectConfig, rootStructure, rootQuery);
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

    private Query CreateRootQuery(SelectQueryConfig selectConfig, EntityStructure rootStructure) {
        var rootTableAlias = aliasStorage.GetTableAlias(selectConfig.EntityName);
        if (selectConfig.AllColumns == true) {
            selectConfig.AddAllColumns(rootStructure);
        }
        var columnExpressions = selectConfig.Expressions.Where(e => e.Type == ExpressionType.Column).ToList();
        var selectBuilder = new SelectColumnBuilder(columnExpressions, rootStructure, rootTableAlias);
        var queryColumns = selectBuilder.BuildAliases();
        var rootQuery = new Query($"{selectConfig.EntityName} as {rootTableAlias}");
        rootQuery.Select(queryColumns);
        var joinsStorage = new JoinsStorage(aliasStorage, rootTableAlias, rootStructure);
        var joinBuilder = new JoinBuilder(rootQuery, aliasStorage, joinsStorage, structureManager);
        joinBuilder.AppendLeftJoins(selectConfig.Expressions, rootTableAlias, rootStructure);
        var subQueryExpressions = selectConfig.Expressions.Where(e => e.Type == ExpressionType.SubQuery).ToList();
        var subQueryBuilder = new SubQueryBuilder(aliasStorage, structureManager);
        subQueryBuilder.AppendSubQueries(rootQuery, subQueryExpressions, rootTableAlias, rootStructure);
        var filterBuilder = new FilterBuilder(rootQuery, aliasStorage, joinsStorage, structureManager, joinsStorage);
        filterBuilder.AppendFilter(selectConfig.Filter);
        var sortBuilder = new SortBuilder(rootQuery, aliasStorage, joinsStorage, structureManager);
        sortBuilder.AppendSorting(selectConfig);
        if (selectConfig.Limit != null && selectConfig.Limit != 0) {
            rootQuery.Limit(selectConfig.Limit.Value);
        }
        return rootQuery;
    }
}
