using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using SqlKata;

namespace Dify.Entity.SelectQuery;

public class JoinBuilder(Query query, TableAliasesStorage aliasesStorage)
{
    public void AppendLeftJoins(List<SelectColumnConfig> columns, string parentTableAlias, 
        EntityStructure parentStructure) {
        var leftColumns = columns.Where(c => parentStructure.Columns
            .Any(ec => ec.Name == c.Path && ec.IsForeignKey)).ToList();
        foreach (var leftColumn in leftColumns) {
            AppendLeftJoin(leftColumn, parentTableAlias, parentStructure);
        }
    }

    private void AppendLeftJoin(SelectColumnConfig columnConfig, string parentTableAlias, 
        EntityStructure parentStructure) {
        var referenceColumn = GetReferenceColumn(parentStructure, columnConfig.Path);
        var tableAlias = aliasesStorage.GetTableAlias(referenceColumn.Name);
        var refEntityStructure = referenceColumn.ForeignKeyStructure!.ReferenceEntityStructure;
        query.LeftJoin(
            $"{refEntityStructure.Name} as {tableAlias}",
            $"{parentTableAlias}.{referenceColumn.DbName}",
            $"{tableAlias}.{parentStructure.PrimaryColumn.Name}");
        SelectQueryUtils.EnsurePrimaryColumnIncluded(columnConfig.Columns, refEntityStructure, tableAlias);
        var queryColumns = new SelectColumnBuilder(columnConfig.Columns, refEntityStructure, tableAlias).Build();
        query.Select(queryColumns);
        AppendLeftJoins(columnConfig.Columns, tableAlias, refEntityStructure);
    }

    private EntityColumnStructure GetReferenceColumn(EntityStructure parentStructure, string path) {
        var referenceColumn = parentStructure.Columns.FirstOrDefault(c => c.Name == path);
        if (referenceColumn?.ForeignKeyStructure == null) {
            throw new ArgumentNullException(nameof(referenceColumn.ForeignKeyStructure),
                $"Foreign key structure not found for column {referenceColumn?.Name}");
        }
        return referenceColumn;
    }
}
