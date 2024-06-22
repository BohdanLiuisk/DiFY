using System.Data;
using System.Text.Json.Serialization;
using Dify.Entity.Descriptor;

namespace Dify.Entity.Structure;

public class EntityColumnStructure
{
    internal EntityColumnStructure(EntityStructure entityStructure, ColumnDescriptor columnDescriptor) {
        var columnType = (DbType)columnDescriptor.Type;
        var dbName = GetColumnDbName(columnDescriptor);
        var isNullable = columnDescriptor.IsNullable ?? false;
        var isUnique = columnDescriptor.IsUnique ?? false;
        var isPrimaryKey = columnDescriptor.IsPrimaryKey ?? false;
        Id = columnDescriptor.Id;
        EntityId = entityStructure.Id;
        EntityStructure = entityStructure;
        Name = columnDescriptor.Name;
        Caption = columnDescriptor.Caption;
        Type = columnType;
        TableName = entityStructure.Name;
        DbName = dbName;
        IsNullable = isNullable;
        IsUnique = isUnique;
        Size = columnDescriptor.Size;
        IsPrimaryKey = isPrimaryKey;
    }
    
    [JsonPropertyName("id")]
    public Guid Id { get; }
    
    [JsonPropertyName("caption")]
    public string Caption { get; }
    
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("dbName")]
    public string DbName { get; }
    
    [JsonPropertyName("entityId")]
    public Guid EntityId { get; }

    [JsonPropertyName("tableName")]
    public string TableName { get; }

    [JsonPropertyName("type")]
    public DbType Type { get; }
    
    [JsonPropertyName("isNullable")]
    public bool IsNullable { get; }
    
    [JsonPropertyName("isPrimaryKey")]
    public bool IsPrimaryKey { get; }
    
    [JsonPropertyName("isUnique")]
    public bool IsUnique { get; }
    
    [JsonPropertyName("size")]
    public int? Size { get; }
    
    [JsonIgnore]
    public EntityStructure EntityStructure { get; set; }
    
    [JsonPropertyName("isForeignKey")]
    public bool IsForeignKey { get; private set; }
    
    [JsonPropertyName("foreignKeyStructure")]
    public EntityForeignKeyStructure? ForeignKeyStructure { get; private set; }

    internal void SetForeignKey(EntityForeignKeyStructure foreignKey) {
        IsForeignKey = true;
        ForeignKeyStructure = foreignKey;
    }
    
    private static string GetColumnDbName(ColumnDescriptor columnDescriptor) {
        if (columnDescriptor.ReferenceEntityId != null) {
            return $"{columnDescriptor.Name}_id";
        }
        return columnDescriptor.Name;
    }
}
