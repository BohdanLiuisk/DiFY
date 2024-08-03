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
        if (selectFilter.Type == Constants.Select.Clause) {
            query.Where(q => AppendClauseFilter(q, selectFilter));
            return;
        }
        if (selectFilter.Type == Constants.Select.Group) {
            query.Where(q => ApplyGroupFilter(q, selectFilter));
        }
    }
    
    private Query AppendFilterInternal(Query filterGroup, SelectFilter selectFilter) {
        if (selectFilter.Type == Constants.Select.Clause) {
            return filterGroup.Where(q => AppendClauseFilter(q, selectFilter));
        }
        if (selectFilter.Type == Constants.Select.Group) {
            return filterGroup.Where(q => ApplyGroupFilter(q, selectFilter));
        }
        return filterGroup;
    }

    private Query ApplyGroupFilter(Query filterGroup, SelectFilter selectFilter) {
        var items = selectFilter.Items;
        for (int i = 0; i < items.Count; i++) {
            var filterItem = items[i];
            AppendFilterInternal(filterGroup, filterItem);
            if (i != items.Count - 1) {
                if (selectFilter.Logical == Constants.Select.Or) {
                    filterGroup.Or();
                }
            }
        }
        return filterGroup;
    }

    private Query AppendClauseFilter(Query filterGroup, SelectFilter selectFilter) {
        if(string.IsNullOrEmpty(selectFilter.Path)) return filterGroup;
        var isSubEntity = GetIsSubEntityFilter(selectFilter.Path);
        if (isSubEntity) {
            var subEntityConfig = GetSubEntityConfig(selectFilter.Path);
            if (subEntityConfig.Operator == Constants.Select.Exists || 
                subEntityConfig.Operator == Constants.Select.NotExists) {
                var alias = joinsStorage.GetTableAlias(subEntityConfig.Name);
                var existsQuery = new Query().From($"{subEntityConfig.Name} as {alias}").As(alias).WhereColumns(
                    $"{alias}.{subEntityConfig.JoinBy}", "=", $"{rootTableAlias}.{subEntityConfig.JoinTo}");
                if (selectFilter.SubFilter != null) {
                    var filterBuilder = new FilterBuilder(existsQuery, alias, new TableJoinsStorage(), structureManager);
                    filterBuilder.AppendFilter(selectFilter.SubFilter);
                }
                if (subEntityConfig.Operator == Constants.Select.Exists) {
                    filterGroup.WhereExists(existsQuery);
                } else { 
                    filterGroup.WhereNotExists(existsQuery);
                }
            }
            if (subEntityConfig.Operator == Constants.Select.Count) {
                var alias = joinsStorage.GetTableAlias(subEntityConfig.Name);
                var existsQuery = new Query().From($"{subEntityConfig.Name} as {alias}").WhereColumns(
                    $"{alias}.{subEntityConfig.JoinBy}", "=", $"{rootTableAlias}.{subEntityConfig.JoinTo}").AsCount();
                if (selectFilter.SubFilter != null) {
                    var filterBuilder = new FilterBuilder(existsQuery, alias, new TableJoinsStorage(), structureManager);
                    filterBuilder.AppendFilter(selectFilter.SubFilter);
                }
                var predicate = selectFilter.SubPredicate;
                var sqlOperator = SelectQueryUtils.MapFilterOperatorToSqlOperator(predicate.Operator);
                filterGroup.WhereSub(existsQuery, sqlOperator, ConvertJsonElement(predicate.Value.Value));
            }
            return filterGroup;
        }
        var singlePredicate = selectFilter.Predicates?.Count == 1 ? selectFilter.Predicates.First() : null;
        var columnPath = $"{rootTableAlias}.{selectFilter.Path}";
        if (singlePredicate != null) {
            ApplyFilterPredicate(filterGroup, columnPath, singlePredicate);
        } else {
            if (selectFilter.Predicates != null) {
                var predicatesList = selectFilter.Predicates.ToList();
                for (int i = 0; i < predicatesList.Count; i++) {
                    var predicate = predicatesList[i];
                    ApplyFilterPredicate(filterGroup, columnPath, predicate);
                    if (i != predicatesList.Count - 1) {
                        if (selectFilter.Logical == Constants.Select.Or) {
                            filterGroup.Or();
                        }
                    }
                }
            }
        }
        return filterGroup;
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

    private static void ApplyFilterPredicate(Query filterGroup, string columnPath, FilterPredicate predicate) {
        switch (predicate) {
            case { Operator: "eq", Value: not null }:
                filterGroup.Where(columnPath, ConvertJsonElement(predicate.Value.Value));
                break;
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
    
    private static object? ConvertJsonElement(JsonElement element) {
        return element.ValueKind switch {
            JsonValueKind.String => element.GetString() ?? string.Empty,
            JsonValueKind.Number => element.TryGetInt32(out int intValue) ? intValue
                : element.TryGetInt64(out long longValue) ? longValue
                : element.TryGetDouble(out double doubleValue) ? doubleValue
                : throw new NotSupportedException($"Unsupported JsonElement value: {element}"),
            JsonValueKind.True or JsonValueKind.False => element.GetBoolean(),
            JsonValueKind.Array => element.EnumerateArray().Select(ConvertJsonElement).ToList(),
            JsonValueKind.Null => null,
            _ => throw new NotSupportedException($"Unsupported JsonElement kind: {element.ValueKind}")
        };
    }
}
