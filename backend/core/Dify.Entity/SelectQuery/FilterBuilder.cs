using System.Text.Json;
using System.Text.RegularExpressions;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Dify.Entity.Utils;
using SqlKata;

namespace Dify.Entity.SelectQuery;

public class FilterBuilder(Query query, string rootTableAlias, TableJoinsStorage joinsStorage, 
    EntityStructureManager structureManager)
{
    private const string SubEntityPathPattern = @"\[(.*?):(.*?):(.*?)\]\.(.*)";
    
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
        if (GetIsSubEntityFilter(selectFilter.Path)) {
            return AppendSubEntityClauseFilter(filterGroup, selectFilter);
        }
        ApplyFilterPredicates(selectFilter, filterGroup);
        return filterGroup;
    }

    private void ApplyFilterPredicates(SelectFilter selectFilter, Query filterGroup) {
        if (selectFilter.Predicates == null || selectFilter.Predicates.Count == 0) return;
        var columnPath = $"{rootTableAlias}.{selectFilter.Path}";
        var predicatesList = selectFilter.Predicates.ToList();
        ApplyFilters(predicatesList, selectFilter.Logical, predicate => 
            ApplyFilterPredicate(filterGroup, columnPath, predicate), filterGroup);
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
        var subEntityConfig = GetSubEntityConfig(selectFilter.Path);
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
            var filterBuilder = new FilterBuilder(existsQuery, alias, new TableJoinsStorage(), structureManager);
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
            var filterBuilder = new FilterBuilder(countQuery, alias, new TableJoinsStorage(), structureManager);
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
    
    private static SubEntityConfig GetSubEntityConfig(string path) {
        var regex = new Regex(SubEntityPathPattern);
        var match = regex.Match(path);
        return new SubEntityConfig {
            Name = match.Groups[1].Value,
            JoinBy = match.Groups[2].Value,
            JoinTo = match.Groups[3].Value,
            Operator = match.Groups[4].Value
        };
    }

    private static bool GetIsSubEntityFilter(string path) {
        var regex = new Regex(SubEntityPathPattern);
        return regex.Match(path).Success;
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
