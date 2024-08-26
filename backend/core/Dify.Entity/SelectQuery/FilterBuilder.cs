using System.Text.Json;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Dify.Entity.Utils;
using SqlKata;

namespace Dify.Entity.SelectQuery;

public class FilterBuilder(Query query, string rootTableAlias, TableJoinsStorage joinsStorage, 
    EntityStructure entityStructure, EntityStructureManager structureManager)
{
    public void AppendFilter(SelectFilter selectFilter) {
        AppendFilterInternal(query, selectFilter);
    }
    
    private void AppendFilterInternal(Query filterGroup, SelectFilter selectFilter) {
        if (selectFilter.Type == Constants.Select.Clause) {
            filterGroup.Where(q => AppendClauseFilter(q, selectFilter));
        }
        if (selectFilter.Type == Constants.Select.Group) {
            filterGroup.Where(q => ApplyGroupFilter(q, selectFilter));
        }
    }

    private Query ApplyGroupFilter(Query filterGroup, SelectFilter selectFilter) {
        ApplyFilters(selectFilter.Items, selectFilter.Logical, item => 
            AppendFilterInternal(filterGroup, item), filterGroup);
        return filterGroup;
    }

    private Query AppendClauseFilter(Query filterGroup, SelectFilter selectFilter) {
        if (string.IsNullOrEmpty(selectFilter.Path)) return filterGroup;
        if (SubEntityConfig.IsSubEntityFilter(selectFilter.Path)) {
            return AppendSubEntityClauseFilter(filterGroup, selectFilter);
        }
        ApplyFilterPredicates(selectFilter, filterGroup);
        return filterGroup;
    }

    private void ApplyFilterPredicates(SelectFilter selectFilter, Query filterGroup) {
        if (selectFilter.Predicates == null || selectFilter.Predicates.Count == 0 
                                            || string.IsNullOrEmpty(selectFilter.Path)) return;
        var pathSegments = selectFilter.Path.Split('.');
        if (pathSegments.Length > 1) {
            var referencePath = string.Join(".", pathSegments.Take(pathSegments.Length - 1));
            var joinMatch = joinsStorage.FindJoinPath(referencePath);
            if (joinMatch.Join != null && string.IsNullOrEmpty(joinMatch.LeftoverPath)) {
                ApplyDirectFilter(selectFilter, filterGroup, $"{joinMatch.Join.Alias}.{pathSegments.Last()}");
            } else {
                var joinAlias = (joinMatch.Join == null)
                    ? BuildJoinPath(rootTableAlias, string.Empty, referencePath, entityStructure)
                    : BuildJoinPath(joinMatch.Join.Alias, joinMatch.Join.JoinPath, joinMatch.LeftoverPath, 
                        joinMatch.Join.PrimaryStructure);
                ApplyDirectFilter(selectFilter, filterGroup, $"{joinAlias}.{pathSegments.Last()}");
            }
            return;
        }
        ApplyDirectFilter(selectFilter, filterGroup, $"{rootTableAlias}.{pathSegments.First()}");
    }
    
    private void ApplyDirectFilter(SelectFilter selectFilter, Query filterGroup, string columnPath) {
        var predicatesList = selectFilter.Predicates!.ToList();
        ApplyFilters(predicatesList, selectFilter.Logical, predicate => 
            ApplyFilterPredicate(filterGroup, columnPath, predicate), filterGroup);
    }

    private string BuildJoinPath(string parentAlias, string existingJoinPath, string fullJoinPath, 
        EntityStructure entityStructure) {
        var pathSegments = fullJoinPath.Split('.');
        var accumulatedPaths = pathSegments
            .Select((_, index) => string.Join('.', pathSegments.Take(index + 1)))
            .ToArray();
        var currentAlias = parentAlias;
        foreach (var fullColumnPath in accumulatedPaths) {
            var columnPath = fullColumnPath.Split('.').Last();
            var foreignKeyStructure = entityStructure.FindForeignKeyStructure(fullColumnPath);
            var refStructure = foreignKeyStructure.ReferenceEntityStructure;
            var completeJoinPath = string.IsNullOrEmpty(existingJoinPath) ? fullColumnPath 
                : $"{existingJoinPath}.{fullColumnPath}";
            var tableAlias = joinsStorage.GetTableAlias(columnPath, completeJoinPath, refStructure, entityStructure);
            query.LeftJoin(
                $"{refStructure.Name} as {tableAlias}",
                $"{currentAlias}.{foreignKeyStructure.PrimaryColumnName}",
                $"{tableAlias}.{refStructure.PrimaryColumn.Name}"
            );
            currentAlias = tableAlias;
        }
        return currentAlias;
    }

    private static void ApplyFilterPredicate(Query filterGroup, string columnPath, FilterPredicate predicate) {
        if (string.IsNullOrEmpty(predicate.Operator)) return;
        if (SelectQueryUtils.GetIsSqlOperator(predicate.Operator) && predicate.Value != null) {
            var compareOperator = SelectQueryUtils.MapFilterOperatorToSqlOperator(predicate.Operator);
            filterGroup.Where(columnPath, compareOperator, ConvertJsonElement(predicate.Value.Value));
            return;
        }
        switch (predicate) {
            case { Operator: "in", Value: not null } 
                when ConvertJsonElement(predicate.Value.Value) is List<object> arrayValue:
                filterGroup.WhereIn(columnPath, arrayValue);
                break;
            case { Operator: "isNull" }:
                filterGroup.WhereNull(columnPath);
                break;
            case { Operator: "isNotNull" }:
                filterGroup.WhereNotNull(columnPath);
                break;
        }
    }
    
    private Query AppendSubEntityClauseFilter(Query filterGroup, SelectFilter selectFilter) {
        if(string.IsNullOrEmpty(selectFilter.Path)) return filterGroup;
        var subEntityConfig = SubEntityConfig.FromFilterPath(selectFilter.Path);
        var alias = joinsStorage.GetTableAlias(subEntityConfig.Name);
        switch (subEntityConfig.Operator) {
            case Constants.Select.Exists:
            case Constants.Select.NotExists:
                return AppendExistsFilter(filterGroup, selectFilter, subEntityConfig, alias);
            case Constants.Select.Count:
                return AppendCountFilter(filterGroup, selectFilter, subEntityConfig, alias);
            default:
                throw new InvalidOperationException($"Unsupported operator: {subEntityConfig.Operator}");
        }
    }
    
    private Query AppendExistsFilter(Query filterGroup, SelectFilter selectFilter, 
        SubEntityConfig subEntityConfig, string alias) {
        var existsQuery = new Query()
            .From($"{subEntityConfig.Name} as {alias}")
            .As(alias)
            .WhereColumns($"{alias}.{subEntityConfig.JoinBy}", "=", $"{rootTableAlias}.{subEntityConfig.JoinTo}");
        if (selectFilter.SubFilter != null) {
            var subStructure = structureManager.FindEntityStructureByName(subEntityConfig.Name)
                .GetAwaiter().GetResult();
            var filterBuilder = new FilterBuilder(existsQuery, alias, new TableJoinsStorage(), 
                subStructure, structureManager);
            filterBuilder.AppendFilter(selectFilter.SubFilter);
        }
        if (subEntityConfig.Operator == Constants.Select.Exists) {
            filterGroup.WhereExists(existsQuery);
        } else {
            filterGroup.WhereNotExists(existsQuery);
        }
        return filterGroup;
    }

    private Query AppendCountFilter(Query filterGroup, SelectFilter selectFilter, 
        SubEntityConfig subEntityConfig, string alias) {
        if(selectFilter.SubPredicate == null || selectFilter.SubPredicate.Value == null) return filterGroup;
        var countQuery = new Query()
            .From($"{subEntityConfig.Name} as {alias}")
            .WhereColumns($"{alias}.{subEntityConfig.JoinBy}", "=", $"{rootTableAlias}.{subEntityConfig.JoinTo}")
            .AsCount();
        if (selectFilter.SubFilter != null) {
            var subStructure = structureManager.FindEntityStructureByName(subEntityConfig.Name)
                .GetAwaiter().GetResult();
            var filterBuilder = new FilterBuilder(countQuery, alias, new TableJoinsStorage(), 
                subStructure, structureManager);
            filterBuilder.AppendFilter(selectFilter.SubFilter);
        }
        var predicate = selectFilter.SubPredicate;
        var sqlOperator = SelectQueryUtils.MapFilterOperatorToSqlOperator(predicate.Operator);
        filterGroup.WhereSub(countQuery, sqlOperator, ConvertJsonElement(predicate.Value.Value));
        return filterGroup;
    }
    
    private static void ApplyFilters<T>(List<T> items, string logicalOp, Action<T> applyFilter, Query filterGroup) {
        for (var i = 0; i < items.Count; i++) {
            applyFilter(items[i]);
            var isNotLastItem = i != items.Count - 1;
            var isLogicalOr = logicalOp == Constants.Select.Or;
            if (isNotLastItem && isLogicalOr) {
                filterGroup.Or();
            }
        }
    }
    
    private static object ConvertJsonElement(JsonElement element) {
        return element.ValueKind switch {
            JsonValueKind.String => element.GetString() ?? string.Empty,
            JsonValueKind.Number => element.TryGetInt32(out int intValue) ? intValue
                : element.TryGetInt64(out long longValue) ? longValue
                : element.TryGetDouble(out double doubleValue) ? doubleValue
                : throw new NotSupportedException($"Unsupported JsonElement value: {element}"),
            JsonValueKind.True or JsonValueKind.False => element.GetBoolean(),
            JsonValueKind.Array => element.EnumerateArray().Select(ConvertJsonElement).ToList(),
            _ => throw new NotSupportedException($"Unsupported JsonElement kind: {element.ValueKind}")
        };
    }
}
