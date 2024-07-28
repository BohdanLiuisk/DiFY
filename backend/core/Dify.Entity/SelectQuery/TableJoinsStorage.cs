using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;

namespace Dify.Entity.SelectQuery;

public class TableJoinsStorage
{
    private readonly Dictionary<string, int> _tableAliases = new();

    private readonly List<JoinInfo> _joins = new();

    public IEnumerable<JoinInfo> Joins => _joins;
    
    public string GetTableAlias(SelectExpression selectExpression, EntityStructure primaryStructure, 
        EntityStructure parentStructure) {
        var alias = GetTableAlias(selectExpression.Path);
        var fullPath = selectExpression.GetFullPath();
        var joinInfo = new JoinInfo(fullPath, alias, primaryStructure, parentStructure);
        _joins.Add(joinInfo);
        return alias;
    }
    
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
        _joins.Clear();
    }
}
