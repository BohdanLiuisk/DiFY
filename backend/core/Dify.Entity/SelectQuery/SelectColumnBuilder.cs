using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;

namespace Dify.Entity.SelectQuery;

public class SelectColumnBuilder(List<SelectColumnConfig> columns, EntityStructure structure, string tableAlias)
{
    public IEnumerable<string> Build() {
        SelectQueryUtils.EnsurePrimaryColumnIncluded(columns, structure, tableAlias);
        foreach (var column in columns) {
            column.Alias = SelectQueryUtils.GetColumnAlias(tableAlias, column.Path);
        }
        var regularColumns = columns.Where(c => structure.Columns.Any(ec => ec.Name == c.Path && !ec.IsForeignKey));
        return regularColumns.Select(c => SelectQueryUtils.GetColumnAsSelect(tableAlias, c.Path));
    }
}
