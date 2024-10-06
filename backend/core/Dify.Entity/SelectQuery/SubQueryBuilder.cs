using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Dify.Entity.Utils;
using SqlKata;

namespace Dify.Entity.SelectQuery;

public class SubQueryBuilder(AliasStorage aliasStorage, EntityStructureManager structureManager)
{
    private readonly JoinsStorage _joinsStorage = new(aliasStorage);
    
    public void AppendSubQueries(Query query, List<SelectExpression> subQueries, string parentTableAlias) {
        foreach (var subQueryExpression in subQueries) {
            var alias = subQueryExpression.Alias;
            if (string.IsNullOrEmpty(alias)) {
                throw new ArgumentException("alias is required for sub query");
            }
            var subQueryBuilder = new SubQueryBuilder(aliasStorage, structureManager);
            var subQuery = subQueryBuilder.BuildSubQuery(subQueryExpression, parentTableAlias);
            subQueryExpression.SelectAlias = SelectQueryUtils.GetColumnAlias(parentTableAlias, alias);
            query.Select(subQuery, subQueryExpression.SelectAlias);
        }
    }
    
    public Query BuildSubQuery(SelectExpression selectExpression, string parentTableAlias) {
        if (string.IsNullOrEmpty(selectExpression.SubEntity)) {
            throw new ArgumentException("subEntity is required for sub query");
        }
        Query subQuery;
        string subTableAlias;
        string subEntityName;
        if (SubEntityConfig.IsSubEntityPath(selectExpression.SubEntity)) {
            var subEntity = SubEntityConfig.FromSubEntityPath(selectExpression.SubEntity);
            subEntityName = subEntity.Name;
            subTableAlias = aliasStorage.GetTableAlias(subEntityName);
            subQuery = new Query($"{subEntityName} as {subTableAlias}")
                .WhereColumns($"{parentTableAlias}.{subEntity.JoinTo}", "=", $"{subTableAlias}.{subEntity.JoinBy}");
        } else {
            subEntityName = selectExpression.SubEntity;
            subTableAlias = aliasStorage.GetTableAlias(subEntityName);
            subQuery = new Query($"{subEntityName} as {subTableAlias}");
        }
        if (!string.IsNullOrEmpty(selectExpression.AggrFunc)) {
            ApplyAggregationFunction(subQuery, selectExpression);
        } else {
            var selectColumn = SelectQueryUtils.GetColumnAsSelect(subTableAlias, selectExpression.Path);
            subQuery.Select(selectColumn);
            ApplySorting(subQuery, subTableAlias, selectExpression);
            subQuery.Limit(1);
        }
        if (selectExpression.Filter == null) return subQuery;
        var subStructure = structureManager.FindEntityStructureByName(subEntityName)
            .GetAwaiter().GetResult();
        var filterBuilder = new FilterBuilder(subQuery, subTableAlias, aliasStorage, _joinsStorage,
            subStructure, structureManager);
        filterBuilder.AppendFilter(selectExpression.Filter);
        return subQuery;
    }
    
    private void ApplyAggregationFunction(Query subQuery, SelectExpression selectExpression) {
        switch (selectExpression.AggrFunc) {
            case Constants.Select.Count:
                subQuery.AsCount();
                break;
            case Constants.Select.Avg:
                subQuery.AsAverage(selectExpression.Path);
                break;
            case Constants.Select.Sum:
                subQuery.AsSum(selectExpression.Path);
                break;
            case Constants.Select.Min:
                subQuery.AsMin(selectExpression.Path);
                break;
            case Constants.Select.Max:
                subQuery.AsMax(selectExpression.Path);
                break;
        }
    }

    private void ApplySorting(Query subQuery, string subTableAlias, SelectExpression selectExpression) {
        if (selectExpression.SubQuerySorting.Count == 0) return;
        var sortingConfigs = selectExpression.SubQuerySorting.OrderBy(c => c.OrderPosition);
        foreach (var sortingConfig in sortingConfigs) {
            var orderByColumn = $"{subTableAlias}.{sortingConfig.OrderBy}";
            switch (sortingConfig.OrderDirection) {
                case Constants.Select.Asc:
                    subQuery.OrderBy(orderByColumn);
                    break;
                case Constants.Select.Desc:
                    subQuery.OrderByDesc(orderByColumn);
                    break;
            }
        }
    }
}
