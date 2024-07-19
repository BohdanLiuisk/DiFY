using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Dify.Entity.Utils;

namespace Dify.Entity.SelectQuery;

public class SelectColumnBuilder(List<SelectExpression> columns, EntityStructure structure, string tableAlias)
{
    public IEnumerable<string> BuildAliases() {
        SelectQueryUtils.EnsurePrimaryColumnIncluded(columns, structure);
        foreach (var column in columns) {
            column.SelectAlias = SelectQueryUtils.GetColumnAlias(tableAlias, column.Path);
        }
        var regularColumns = columns.Where(c => structure.Columns
            .Any(ec => ec.Name == c.Path && !ec.IsForeignKey));
        return regularColumns.Select(c => SelectQueryUtils.GetColumnAsSelect(tableAlias, c.Path));
    }
}
