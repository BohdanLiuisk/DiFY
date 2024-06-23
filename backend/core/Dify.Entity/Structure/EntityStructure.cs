using System.Text.Json;

namespace Dify.Entity.Structure;

public class EntityStructure
{
    private IList<EntityColumnStructure> _columns = new List<EntityColumnStructure>();
    
    private IList<EntityForeignKeyStructure> _foreignKeys = new List<EntityForeignKeyStructure>();

    private IList<EntityIndexStructure> _indexes = new List<EntityIndexStructure>();

    internal EntityStructure(Guid id, string name, string caption) {
        Id = id;
        Name = name;
        Caption = caption;
    }
    
    public Guid Id { get; }
    
    public string Name { get; }
    
    public string Caption { get; }
    
    public EntityColumnStructure PrimaryColumn => _columns.First(c => c.IsPrimaryKey);
    
    internal string ColumnsJson { get; private set; } = "[]";
    
    internal string ForeignKeysJson { get; private set; } = "[]";
    
    internal string IndexesJson { get; private set; } = "[]";
    
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

    internal void DeserializeProperties() {
        DeserializeColumns();
        DeserializeForeignKeys();
        DeserializeIndexes();
        LinkForeignKeysToColumns();
    }

    private void DeserializeColumns() {
        var columns = JsonSerializer.Deserialize<IEnumerable<EntityColumnStructure>>(ColumnsJson);
        if (columns == null) return;
        _columns = columns.ToList();
        foreach (var column in _columns) {
            column.EntityStructure = this;
        }
    }

    private void DeserializeForeignKeys() {
        var foreignKeys = JsonSerializer.Deserialize<IEnumerable<EntityForeignKeyStructure>>(ForeignKeysJson);
        if (foreignKeys == null) return;
        _foreignKeys = foreignKeys.ToList();
        foreach (var foreignKey in _foreignKeys) {
            foreignKey.PrimaryEntity = this;
            foreignKey.PrimaryColumn = _columns.First(c => c.DbName == foreignKey.PrimaryColumnName);
        }
    }

    private void DeserializeIndexes() {
        var indexes = JsonSerializer.Deserialize<IEnumerable<EntityIndexStructure>>(IndexesJson);
        if (indexes == null) return;
        _indexes = indexes.ToList();
        foreach (var index in _indexes) {
            index.EntityStructure = this;
        }
    }

    private void LinkForeignKeysToColumns() {
        foreach (var column in _columns) {
            if (column is { IsForeignKey: true, ForeignKeyStructureId: not null }) {
                column.ForeignKeyStructure = _foreignKeys.First(f => f.Id == column.ForeignKeyStructureId);
            }
        }
    }
}
