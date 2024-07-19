namespace Dify.Entity.SelectQuery;

public class TableJoinsStorage
{
    private readonly Dictionary<string, int> _tableAliases = new();
    
    public string GetTableAlias(string refColumnPath) {
        if (_tableAliases.TryGetValue(refColumnPath, out var aliasCount)) {
            var tableAlias = $"{refColumnPath}_{aliasCount}";
            _tableAliases[refColumnPath] = ++aliasCount;
            return tableAlias;
        }
        _tableAliases[refColumnPath] = 1;
        return $"{refColumnPath}";
    }

    public void Clear() {
        _tableAliases.Clear();
    }
}
