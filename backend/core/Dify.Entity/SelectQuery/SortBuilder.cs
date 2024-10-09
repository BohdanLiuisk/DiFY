using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Dify.Entity.Utils;
using SqlKata;

namespace Dify.Entity.SelectQuery;

public class SortBuilder(Query query, AliasStorage aliasStorage, JoinsStorage joinsStorage, 
    EntityStructureManager structureManager)
{
    public void AppendSorting(SelectQueryConfig selectConfig) {
        if(selectConfig.Sorting == null) return;
        var joinBuilder = new JoinBuilder(query, aliasStorage, joinsStorage, structureManager);
        foreach (var sortConfig in selectConfig.Sorting.OrderBy(s => s.OrderPosition)) {
            var (joinPath, orderByColumn) = SplitPathAtLastDot(sortConfig.OrderBy);
            string orderByExpression;
            if (string.IsNullOrEmpty(joinPath)) {
                var matchingExpression = selectConfig.Expressions.FirstOrDefault(
                    e => e.Alias == orderByColumn || e.Path == orderByColumn);
                orderByExpression = matchingExpression == null 
                    ? $"{joinsStorage.StructureAlias}.{orderByColumn}" 
                    : matchingExpression.SelectAlias ?? orderByColumn;
            } else {
                var existingJoin = selectConfig.FindExpressionByPath(joinPath);
                var joinAlias = joinBuilder.BuildJoinAlias(joinPath);
                if (existingJoin != null) {
                    var matchingColumn = existingJoin.Columns.FirstOrDefault(
                        c => c.Alias == orderByColumn || c.Path == orderByColumn);
                    orderByExpression = matchingColumn?.SelectAlias ?? $"{joinAlias}.{orderByColumn}";
                } else {
                    orderByExpression = $"{joinAlias}.{orderByColumn}";
                }
            }
            ApplyOrderBy(sortConfig, orderByExpression);
        }
    }

    private void ApplyOrderBy(SortConfig sortConfig, string orderByExpression) {
        if (sortConfig.OrderDirection == Constants.Select.Asc) {
            query.OrderBy(orderByExpression);
        } else if (sortConfig.OrderDirection == Constants.Select.Desc) {
            query.OrderByDesc(orderByExpression);
        }
    }
    
    private static (string? joinPart, string orderByColumn) SplitPathAtLastDot(string input) {
        var lastDotIndex = input.LastIndexOf('.');
        if (lastDotIndex == -1) return (null, input);
        var joinPart = input[..lastDotIndex];
        var orderByPart = input[(lastDotIndex + 1)..];
        return (joinPart, orderByPart);
    }
}
