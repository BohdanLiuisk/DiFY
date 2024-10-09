using Dify.Entity.SelectQuery.Enums;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Dify.Entity.Utils;
using SqlKata;

namespace Dify.Entity.SelectQuery;

public class JoinBuilder(Query query, AliasStorage aliasStorage, JoinsStorage joinsStorage, 
    EntityStructureManager structureManager)
{
    public void AppendLeftJoins(List<SelectExpression> leftColumns, string parentTableAlias, 
        EntityStructure parentStructure) {
        foreach (var leftColumn in leftColumns) {
            AppendLeftJoin(leftColumn, parentTableAlias, parentStructure);
        }
    }

    private void AppendLeftJoin(SelectExpression expression, string parentTableAlias, EntityStructure parentStructure) {
        var referenceColumn = parentStructure.Columns.FirstOrDefault(c => c.Name == expression.Path);
        if (referenceColumn?.ForeignKeyStructure == null) return;
        var refEntityStructure = referenceColumn.ForeignKeyStructure.ReferenceEntityStructure;
        var tableAlias = joinsStorage.BuildJoinAlias(expression, refEntityStructure, parentStructure);
        query.LeftJoin(
            $"{refEntityStructure.Name} as {tableAlias}",
            $"{parentTableAlias}.{referenceColumn.DbName}",
            $"{tableAlias}.{parentStructure.PrimaryColumn.Name}");
        SelectQueryUtils.EnsurePrimaryColumnIncluded(expression.Columns, refEntityStructure);
        var columnExpressions = expression.Columns.Where(c => c.Type == ExpressionType.Column).ToList();
        var queryColumns = new SelectColumnBuilder(columnExpressions, refEntityStructure, tableAlias).BuildAliases();
        query.Select(queryColumns);
        var subQueryExpressions = expression.Columns.Where(c => c.Type == ExpressionType.SubQuery).ToList();
        var subQueryBuilder = new SubQueryBuilder(aliasStorage, structureManager);
        subQueryBuilder.AppendSubQueries(query, subQueryExpressions, tableAlias, refEntityStructure);
        if (expression.Columns.Count != 0) {
            AppendLeftJoins(expression.Columns, tableAlias, refEntityStructure);
        }
    }

    public string BuildJoinAlias(string referencePath) {
        var joinMatch = joinsStorage.FindJoinPath(referencePath);
        if (joinMatch.Join != null && string.IsNullOrEmpty(joinMatch.LeftoverPath)) {
            return joinMatch.Join.Alias;
        }
        var joinAlias = joinMatch.Join == null
            ? BuildJoinPath(joinsStorage.StructureAlias, string.Empty, referencePath, joinsStorage.EntityStructure)
            : BuildJoinPath(joinMatch.Join.Alias, joinMatch.Join.JoinPath, joinMatch.LeftoverPath, 
                joinMatch.Join.PrimaryStructure);
        return joinAlias;
    }
    
    public ColumnPathInfo BuildColumnPathInfo(string path) {
        var pathSegments = path.Split('.');
        if (pathSegments.Length <= 1) {
            var columnStructure = joinsStorage.EntityStructure.FindColumn(path);
            var columnPath = $"{joinsStorage.StructureAlias}.{columnStructure.DbName}";
            return new ColumnPathInfo(Path: columnPath, ColumnStructure: columnStructure);
        }
        var referencePath = string.Join(".", pathSegments.Take(pathSegments.Length - 1));
        var joinMatch = joinsStorage.FindJoinPath(referencePath);
        var columnName = pathSegments.Last();
        if (joinMatch.Join != null && string.IsNullOrEmpty(joinMatch.LeftoverPath)) {
            var columnStructure = joinMatch.Join.PrimaryStructure.FindColumn(columnName);
            return new ColumnPathInfo(
                Path: $"{joinMatch.Join.Alias}.{columnStructure.DbName}", 
                ColumnStructure: columnStructure);
        } else {
            var joinAlias = joinMatch.Join == null
                ? BuildJoinPath(joinsStorage.StructureAlias, string.Empty, referencePath, joinsStorage.EntityStructure)
                : BuildJoinPath(joinMatch.Join.Alias, joinMatch.Join.JoinPath, joinMatch.LeftoverPath, 
                    joinMatch.Join.PrimaryStructure);
            var columnStructure = joinsStorage.FindJoinPath(referencePath).Join!.PrimaryStructure
                .FindColumn(columnName);
            return new ColumnPathInfo(Path: $"{joinAlias}.{columnStructure.DbName}", columnStructure);
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
}
