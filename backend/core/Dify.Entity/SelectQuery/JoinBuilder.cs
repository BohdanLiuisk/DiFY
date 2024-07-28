using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Dify.Entity.Utils;
using SqlKata;

namespace Dify.Entity.SelectQuery;

public class JoinBuilder(Query query, TableJoinsStorage joinsStorage)
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
        var tableAlias = joinsStorage.GetTableAlias(expression, refEntityStructure, parentStructure);
        query.LeftJoin(
            $"{refEntityStructure.Name} as {tableAlias}",
            $"{parentTableAlias}.{referenceColumn.DbName}",
            $"{tableAlias}.{parentStructure.PrimaryColumn.Name}");
        SelectQueryUtils.EnsurePrimaryColumnIncluded(expression.Columns, refEntityStructure);
        var queryColumns = new SelectColumnBuilder(expression.Columns, refEntityStructure, tableAlias).BuildAliases();
        query.Select(queryColumns);
        if (expression.Columns.Count != 0) {
            AppendLeftJoins(expression.Columns, tableAlias, refEntityStructure);
        }
    }
}
