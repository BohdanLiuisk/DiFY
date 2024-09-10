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
        if(referenceColumn?.ForeignKeyStructure == null) return;
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
        subQueryBuilder.AppendSubQueries(query, subQueryExpressions, tableAlias);
        if (expression.Columns.Count != 0) {
            AppendLeftJoins(expression.Columns, tableAlias, refEntityStructure);
        }
    }
}
