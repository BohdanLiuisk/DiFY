using System.Text.Json;
using Dify.Entity.SelectQuery.Enums;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Dify.Entity.Utils;
using SqlKata;
using static Dify.Entity.Utils.Constants;

namespace Dify.Entity.SelectQuery;

public class FilterBuilder(Query query, string rootTableAlias, AliasStorage aliasStorage, JoinsStorage joinsStorage,
    EntityStructure entityStructure, EntityStructureManager structureManager)
{
    public void AppendFilter(SelectFilter selectFilter) {
        AppendFilterInternal(query, selectFilter);
    }
    
    private void AppendFilterInternal(Query filterGroup, SelectFilter selectFilter) {
        if (selectFilter.Type == Select.Clause) {
            filterGroup.Where(q => AppendClauseFilter(q, selectFilter));
        }
        if (selectFilter.Type == Select.Group) {
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
        if (selectFilter.Predicates.Count == 0 || string.IsNullOrEmpty(selectFilter.Path)) return;
        var pathInfo = BuildColumnPathInfo(selectFilter.Path);
        ApplyFilters(selectFilter.Predicates, selectFilter.Logical, predicate => 
            ApplyFilterPredicate(filterGroup, pathInfo, predicate), filterGroup);
    }

    private ColumnPathInfo BuildColumnPathInfo(string path) {
        var pathSegments = path.Split('.');
        if (pathSegments.Length <= 1) {
            var columnPath = $"{rootTableAlias}.{path}";
            var columnStructure = entityStructure.FindColumn(path);
            return new ColumnPathInfo(Path: columnPath, ColumnStructure: columnStructure);
        }
        var referencePath = string.Join(".", pathSegments.Take(pathSegments.Length - 1));
        var joinMatch = joinsStorage.FindJoinPath(referencePath);
        var columnName = pathSegments.Last();
        if (joinMatch.Join != null && string.IsNullOrEmpty(joinMatch.LeftoverPath)) {
            var columnStructure = joinMatch.Join.PrimaryStructure.FindColumn(columnName);
            return new ColumnPathInfo(Path: $"{joinMatch.Join.Alias}.{columnName}", ColumnStructure: columnStructure);
        } else {
            var joinAlias = joinMatch.Join == null
                ? BuildJoinPath(rootTableAlias, string.Empty, referencePath, entityStructure)
                : BuildJoinPath(joinMatch.Join.Alias, joinMatch.Join.JoinPath, joinMatch.LeftoverPath, 
                    joinMatch.Join.PrimaryStructure);
            var columnStructure = joinsStorage.FindJoinPath(referencePath).Join!.PrimaryStructure
                .FindColumn(columnName);
            return new ColumnPathInfo(Path: $"{joinAlias}.{columnName}", columnStructure);
        }
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
            var tableAlias = joinsStorage.BuildJoinAlias(columnPath, completeJoinPath, refStructure, entityStructure);
            query.LeftJoin(
                $"{refStructure.Name} as {tableAlias}",
                $"{currentAlias}.{foreignKeyStructure.PrimaryColumnName}",
                $"{tableAlias}.{refStructure.PrimaryColumn.Name}"
            );
            currentAlias = tableAlias;
        }
        return currentAlias;
    }

    private void ApplyFilterPredicate(Query filterGroup, ColumnPathInfo pathInfo, FilterPredicate predicate) {
        switch (predicate.ValueType) {
            case PredicateValueType.Value:
                ApplyFilterByValue(filterGroup, pathInfo, predicate);
                break;
            case PredicateValueType.Column:
                ApplyWhereColumnsFilter(filterGroup, pathInfo, predicate);
                break;
            case PredicateValueType.SubQuery:
                ApplySubQueryFilter(filterGroup, pathInfo, predicate);
                break;
        }
    }

    private static void ApplyFilterByValue(Query filterGroup, ColumnPathInfo pathInfo, FilterPredicate predicate) {
        if (string.IsNullOrEmpty(predicate.Operator)) return;
        if (DbTypeUtils.GetIsDateTimeType(pathInfo.ColumnStructure.Type)) {
            ApplyDateFilter(filterGroup, pathInfo.Path, predicate);
        } else if (DbTypeUtils.GetIsStringType(pathInfo.ColumnStructure.Type) && IsStringComparison(predicate)) {
            ApplyStringFilter(filterGroup, pathInfo.Path, predicate);
        } else {
            ApplyWhereFilter(filterGroup, pathInfo.Path, predicate);
        }
    }

    private void ApplyWhereColumnsFilter(Query filterGroup, ColumnPathInfo pathInfo, FilterPredicate predicate) {
        if (!SelectQueryUtils.GetIsSqlComparisonOperator(predicate.Operator)) return;
        var comparePath = ConvertJsonElement(predicate.Value).ToString();
        if (comparePath == null) {
            throw new ArgumentNullException(nameof(predicate.Value), "Compared column path cannot be empty");
        }
        var compareOperator = SelectQueryUtils.MapFilterOperatorToSqlOperator(predicate.Operator);
        var comparePathInfo = BuildColumnPathInfo(comparePath);
        filterGroup.WhereColumns(pathInfo.Path, compareOperator, comparePathInfo.Path);
    }

    private void ApplySubQueryFilter(Query filterGroup, ColumnPathInfo pathInfo, FilterPredicate predicate) {
        if (predicate.SubQuery == null) return;
        if (!SelectQueryUtils.GetIsSqlComparisonOperator(predicate.Operator)) return;
        var subQueryBuilder = new SubQueryBuilder(aliasStorage, structureManager);
        var subQuery = subQueryBuilder.BuildSubQuery(predicate.SubQuery, rootTableAlias);
        var compareOperator = SelectQueryUtils.MapFilterOperatorToSqlOperator(predicate.Operator);
        filterGroup.Where(pathInfo.Path, compareOperator, subQuery);
    }

    private static void ApplyWhereFilter(Query filterGroup, string columnPath, FilterPredicate predicate) {
        if (SelectQueryUtils.GetIsSqlComparisonOperator(predicate.Operator)) {
            ApplyComparisonFilter(filterGroup, columnPath, predicate);
        } else {
            ApplyNonComparisonFilter(filterGroup, columnPath, predicate);
        }
    }
    
    private static void ApplyComparisonFilter(Query filterGroup, string columnPath, FilterPredicate predicate) {
        var compareOperator = SelectQueryUtils.MapFilterOperatorToSqlOperator(predicate.Operator);
        var convertedValue = ConvertJsonElement(predicate.Value);
        filterGroup.Where(columnPath, compareOperator, convertedValue);
    }
    
    private static void ApplyNonComparisonFilter(Query filterGroup, string columnPath, FilterPredicate predicate) {
        switch (predicate.Operator) {
            case Select.In when ConvertJsonElement(predicate.Value) is List<object> arrayValue:
                filterGroup.WhereIn(columnPath, arrayValue);
                break;
            case Select.IsNull:
                filterGroup.WhereNull(columnPath);
                break;
            case Select.IsNotNull:
                filterGroup.WhereNotNull(columnPath);
                break;
        }
    }

    private static void ApplyStringFilter(Query filterGroup, string columnPath, FilterPredicate predicate) {
        if(predicate.Value == null) return;
        var predicateValue = ConvertJsonElement(predicate.Value);
        var caseSensitive = predicate.CaseSensitive ?? false;
        switch (predicate.Operator) {
            case Select.Contains:
                filterGroup.WhereContains(columnPath, predicateValue, caseSensitive);
                break;
            case Select.NotContains:
                filterGroup.WhereNotContains(columnPath, predicateValue, caseSensitive);
                break;
            case Select.StartsWith:
                filterGroup.WhereStarts(columnPath, predicateValue, caseSensitive);
                break;
            case Select.NotStartsWith:
                filterGroup.WhereNotStarts(columnPath, predicateValue, caseSensitive);
                break;
            case Select.EndsWith:
                filterGroup.WhereEnds(columnPath, predicateValue, caseSensitive);
                break;
            case Select.NotEndsWith:
                filterGroup.WhereNotEnds(columnPath, predicateValue, caseSensitive);
                break;
        }
    }
    
    private static void ApplyDateFilter(Query filterGroup, string columnPath, FilterPredicate predicate) {
        if (IsNonComparison(predicate)) {
            ApplyNonComparisonFilter(filterGroup, columnPath, predicate);
            return;
        }
        if (!SelectQueryUtils.GetIsSqlComparisonOperator(predicate.Operator)) return;
        var compareOperator = SelectQueryUtils.MapFilterOperatorToSqlOperator(predicate.Operator);
        var predicateValue = ConvertJsonElement(predicate.Value);
        if (!string.IsNullOrEmpty(predicate.DatePart)) {
            ApplyDatePartFilter(filterGroup, columnPath, compareOperator, predicateValue, predicate.DatePart);
        } else if (DateTime.TryParse(predicateValue.ToString(), out var dateValue)) {
            filterGroup.WhereDate(columnPath, compareOperator, dateValue);
        } else {
            throw new InvalidCastException($"Couldn't cast {predicateValue} to datetime");
        }
    }
    
    private static void ApplyDatePartFilter(Query filterGroup, string columnPath, string compareOperator, 
        object predicateValue, string datePart) {
        switch (datePart) {
            case "time":
                var timeSpan = TimeSpan.Parse(predicateValue.ToString() ?? string.Empty);
                filterGroup.WhereTime(columnPath, compareOperator, timeSpan);
                break;
            default:
                filterGroup.WhereDatePart(datePart, columnPath, compareOperator, predicateValue);
                break;
        }
    }
    
    private Query AppendSubEntityClauseFilter(Query filterGroup, SelectFilter selectFilter) {
        if(string.IsNullOrEmpty(selectFilter.Path)) return filterGroup;
        var subEntityConfig = SubEntityConfig.FromFilterPath(selectFilter.Path);
        var alias = aliasStorage.GetTableAlias(subEntityConfig.Name);
        switch (subEntityConfig.Operator) {
            case Select.Exists:
            case Select.NotExists:
                return AppendExistsFilter(filterGroup, selectFilter, subEntityConfig, alias);
            case Select.Count:
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
            var filterBuilder = new FilterBuilder(existsQuery, alias, aliasStorage, new JoinsStorage(aliasStorage), 
                subStructure, structureManager);
            filterBuilder.AppendFilter(selectFilter.SubFilter);
        }
        if (subEntityConfig.Operator == Select.Exists) {
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
            var filterBuilder = new FilterBuilder(countQuery, alias, aliasStorage, new JoinsStorage(aliasStorage), 
                subStructure, structureManager);
            filterBuilder.AppendFilter(selectFilter.SubFilter);
        }
        var predicate = selectFilter.SubPredicate;
        var sqlOperator = SelectQueryUtils.MapFilterOperatorToSqlOperator(predicate.Operator);
        filterGroup.WhereSub(countQuery, sqlOperator, ConvertJsonElement(predicate.Value));
        return filterGroup;
    }
    
    private static bool IsNonComparison(FilterPredicate predicate) => 
        predicate.Operator is Select.IsNull or Select.IsNotNull or Select.In;

    private static bool IsStringComparison(FilterPredicate predicate) =>
        predicate.Operator is Select.Contains or Select.NotContains or Select.StartsWith
            or Select.NotStartsWith or Select.EndsWith or Select.NotEndsWith;
    
    private static void ApplyFilters<T>(List<T> items, string logicalOp, Action<T> applyFilter, Query filterGroup) {
        for (var i = 0; i < items.Count; i++) {
            applyFilter(items[i]);
            var isNotLastItem = i != items.Count - 1;
            var isLogicalOr = logicalOp == Select.Or;
            if (isNotLastItem && isLogicalOr) {
                filterGroup.Or();
            }
        }
    }
    
    private static object ConvertJsonElement(JsonElement? element) {
        if (element == null) {
            throw new ArgumentNullException(nameof(element), "Unsupported filter value");
        }
        var jsonElement = element.Value;
        return jsonElement.ValueKind switch {
            JsonValueKind.String => jsonElement.GetString() ?? string.Empty,
            JsonValueKind.Number => jsonElement.TryGetInt32(out int intValue) ? intValue
                : jsonElement.TryGetInt64(out long longValue) ? longValue
                : jsonElement.TryGetDouble(out double doubleValue) ? doubleValue
                : throw new NotSupportedException($"Unsupported JsonElement value: {element}"),
            JsonValueKind.True or JsonValueKind.False => jsonElement.GetBoolean(),
            JsonValueKind.Array => jsonElement.EnumerateArray().Select(e => ConvertJsonElement(e)).ToList(),
            _ => throw new NotSupportedException($"Unsupported JsonElement kind: {jsonElement.ValueKind}")
        };
    }
}
