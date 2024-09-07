using Dify.Entity.SelectQuery.Enums;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;

namespace Dify.Entity.Utils;

public static class SelectQueryUtils
{
    private static readonly Dictionary<string, string> _filterOperatorToSql = new() {
        { "eq", "=" },
        { "ne", "<>" },
        { "gt", ">" },
        { "gte", ">=" },
        { "lt", "<" },
        { "lte", "<=" }
    };
    
    public static string GetColumnAsSelect(string tableAlias, string path) {
        var columnAlias = GetColumnAlias(tableAlias, path);
        return $"{tableAlias}.{path} as {columnAlias}";
    }

    public static string GetColumnAlias(string tableAlias, string path) {
        return $"{tableAlias}_{path}";
    }

    public static string GetExpressionAlias(SelectExpression expression) {
        return expression.Type switch {
            ExpressionType.Column => !string.IsNullOrEmpty(expression.Alias) ? expression.Alias : expression.Path,
            ExpressionType.Function or ExpressionType.SubQuery when string.IsNullOrEmpty(expression.Alias) =>
                throw new InvalidOperationException("Alias is required"),
            ExpressionType.Function or ExpressionType.SubQuery => expression.Alias,
            _ => throw new ArgumentOutOfRangeException(nameof(expression.Type))
        };
    }
    
    public static IEnumerable<IDictionary<string, object>> GetResultRows(IEnumerable<dynamic> rows) {
        return rows.Select(row => (IDictionary<string, object>)row);
    }
    
    public static void EnsurePrimaryColumnIncluded(List<SelectExpression> columns, EntityStructure entityStructure) {
        var primaryColumnName = entityStructure.PrimaryColumn.Name;
        if (!columns.Any(c => c.Path == primaryColumnName && c.Type == ExpressionType.Column)) {
            columns.Insert(0, SelectExpression.Column(primaryColumnName));
        }
    }

    public static bool GetIsSqlComparisonOperator(string filterOperator) {
        return _filterOperatorToSql.ContainsKey(filterOperator);
    }

    public static string MapFilterOperatorToSqlOperator(string filterOperator) {
        return _filterOperatorToSql[filterOperator];
    } 
}
