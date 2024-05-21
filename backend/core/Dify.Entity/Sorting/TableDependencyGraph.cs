using Dify.Entity.Descriptor;

namespace Dify.Entity.Sorting;

public class TableDependencyGraph
{
    private readonly Dictionary<string, TableDescriptor> _tables = new();
    
    private readonly Dictionary<string, List<string>> _dependencies = new();
    
    public TableDependencyGraph(IList<TableDescriptor> tableDefinitions) {
        InitDependencies(tableDefinitions);
    }
    
    public IList<TableDescriptor> SortTables() {
        var sorted = new List<TableDescriptor>();
        var visited = new HashSet<string>();
        var visiting = new HashSet<string>();
        foreach (var table in _tables.Values) {
            if (!visited.Contains(table.Name)) {
                Visit(table.Name, visited, visiting, sorted);
            }
        }
        sorted.Reverse();
        return sorted;
    }

    private void Visit(string tableCode, HashSet<string> visited, HashSet<string> visiting, 
        IList<TableDescriptor> sorted) {
        if (visiting.Contains(tableCode)) return;
        if (!visited.Contains(tableCode)) {
            visiting.Add(tableCode);
            if (_dependencies.TryGetValue(tableCode, out var dependency)) {
                foreach (var dependentTableCode in dependency) {
                    Visit(dependentTableCode, visited, visiting, sorted);
                }
            }
            visiting.Remove(tableCode);
            visited.Add(tableCode);
            sorted.Add(_tables[tableCode]);
        }
    }
    
    private void InitDependencies(IList<TableDescriptor> tableDefinitions) {
        foreach (var table in tableDefinitions) {
            _tables[table.Name] = table;
            foreach (var column in table.Columns) {
                if (column.ForeignTable != null) {
                    if (!_dependencies.ContainsKey(column.ForeignTable.Name)) {
                        _dependencies[column.ForeignTable.Name] = new List<string>();
                    }
                    _dependencies[column.ForeignTable.Name].Add(table.Name);
                }
            }
        }
    }
}
