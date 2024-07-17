using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;

namespace Dify.Entity.SelectQuery;

public static class SelectQueryUtils
{
    public static void EnsurePrimaryColumnIncluded(List<SelectColumnConfig> columns, EntityStructure parentStructure, 
        string tableAlias) {
        var primaryColumnName = parentStructure.PrimaryColumn.Name;
        if (!columns.Select(c => c.Path).Contains(primaryColumnName)) {
            columns.Add(new SelectColumnConfig(primaryColumnName, tableAlias));
        }
    }

    public static string GetColumnAsSelect(string tableAlias, string path) {
        var columnAlias = GetColumnAlias(tableAlias, path);
        return $"{tableAlias}.{path} as {columnAlias}";
    }

    public static string GetColumnAlias(string tableAlias, string path) {
        return $"{tableAlias}_{path}";
    }
}
