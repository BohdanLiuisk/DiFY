using System.Text.Json.Serialization;

namespace Dify.Entity.Structure;

public class EntityForeignKeyStructure
{
    internal EntityForeignKeyStructure(EntityStructure entityStructure, EntityColumnStructure columnStructure,
        Guid referenceEntityId) {
        Id = Guid.NewGuid();
        PrimaryEntityName = entityStructure.Name;
        PrimaryEntityId = entityStructure.Id;
        PrimaryColumnName = columnStructure.DbName;
        ReferenceEntityId = referenceEntityId;
        PrimaryColumn = columnStructure;
        PrimaryEntity = entityStructure;
    }
    
    [JsonConstructor]
    internal EntityForeignKeyStructure(Guid id, string primaryEntityName, Guid primaryEntityId, string primaryColumnName,
        Guid referenceEntityId, string referenceEntityName, string referenceColumnName, string foreignKeyName) {
        Id = id;
        PrimaryEntityName = primaryEntityName;
        PrimaryEntityId = primaryEntityId;
        PrimaryColumnName = primaryColumnName;
        ReferenceEntityId = referenceEntityId;
        ReferenceEntityName = referenceEntityName;
        ReferenceColumnName = referenceColumnName;
        ForeignKeyName = foreignKeyName;
    }
    
    [JsonPropertyName("id")]
    public Guid Id { get; }
    
    [JsonPropertyName("primaryEntityName")]
    public string PrimaryEntityName { get; }

    [JsonPropertyName("primaryEntityId")]
    public Guid PrimaryEntityId { get; }

    [JsonPropertyName("primaryColumnName")]
    public string PrimaryColumnName { get; }

    [JsonPropertyName("referenceEntityId")]
    public Guid ReferenceEntityId { get; }

    [JsonPropertyName("referenceEntityName")]
    public string ReferenceEntityName { get; private set; } = null!;

    [JsonPropertyName("referenceColumnName")]
    public string ReferenceColumnName { get; private set; } = null!;
    
    [JsonPropertyName("foreignKeyName")]
    public string ForeignKeyName { get; private set; } = null!;
    
    [JsonIgnore]
    public EntityColumnStructure PrimaryColumn { get; internal set; }

    [JsonIgnore]
    public EntityStructure PrimaryEntity { get; internal set; }
    
    [JsonIgnore]
    public EntityStructure ReferenceEntityStructure { get; internal set; } = null!;
    
    internal void LinkReferenceEntity(EntityStructure referenceEntity) {
        ReferenceEntityStructure = referenceEntity;
        ReferenceColumnName = referenceEntity.PrimaryColumn.Name;
        ReferenceEntityName = referenceEntity.Name;
        var fkName = GetForeignKeyName(PrimaryEntityName, ReferenceEntityName, PrimaryColumnName);
        ForeignKeyName = fkName;
    }
    
    private static string GetForeignKeyName(string tableName, string referenceTable, string columnName) {
        return $"fk_{tableName}_{referenceTable}_{columnName}";
    }
}
