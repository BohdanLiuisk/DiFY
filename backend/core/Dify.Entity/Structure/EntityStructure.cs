using System.Text.Json;

namespace Dify.Entity.Structure;

public class EntityStructure
{
    private readonly IList<EntityColumnStructure> _columns = new List<EntityColumnStructure>();
    
    private readonly IList<EntityForeignKeyStructure> _foreignKeys = new List<EntityForeignKeyStructure>();

    private readonly IList<EntityIndexStructure> _indexes = new List<EntityIndexStructure>();

    internal EntityStructure(Guid id, string name, string caption) {
        Id = id;
        Name = name;
        Caption = caption;
    }
    
    public Guid Id { get; }
    
    public string Name { get; }
    
    public string Caption { get; }
    
    public EntityColumnStructure PrimaryColumn => _columns.Single(c => c.IsPrimaryKey);
    
    public string ColumnsJson { get; private set; } = string.Empty;
    
    public string ForeignKeysJson { get; private set; } = string.Empty;
    
    public string IndexesJson { get; private set; } = string.Empty;
    
    public IEnumerable<EntityColumnStructure> Columns => _columns;
    
    public IEnumerable<EntityForeignKeyStructure> ForeignKeys => _foreignKeys;
    
    public IEnumerable<EntityIndexStructure> Indexes => _indexes;
    
    public void AddColumn(EntityColumnStructure columnStructure) {
        _columns.Add(columnStructure);
        ColumnsJson = JsonSerializer.Serialize(_columns);
    }
    
    public void AddForeignKey(EntityForeignKeyStructure foreignKeyStructure) {
        _foreignKeys.Add(foreignKeyStructure);
        ForeignKeysJson = JsonSerializer.Serialize(_foreignKeys);
    }

    public void AddIndex(EntityIndexStructure indexStructure) {
        _indexes.Add(indexStructure);
        IndexesJson = JsonSerializer.Serialize(_indexes);
    }
}
