using System.Text.Json.Serialization;

namespace Dify.Entity.Structure;

public class EntityIndexStructure
{
    private EntityIndexStructure() { }
    
    [JsonIgnore]
    public EntityStructure EntityStructure { get; private set; }
    
    [JsonPropertyName("entityName")]
    public string EntityName { get; private set; }
    
    [JsonPropertyName("name")]
    public string Name { get; private set; }
    
    [JsonPropertyName("isUnique")]
    public bool IsUnique { get; private set; }
    
    [JsonPropertyName("columns")]
    public IEnumerable<string> Columns { get; private set; }

    internal static EntityIndexStructure CreateUniqueIndex(EntityStructure entityStructure, string columnName) {
        return new EntityIndexStructure {
            EntityStructure = entityStructure,
            EntityName = entityStructure.Name,
            Name = GetUniqueIndexName(entityStructure.Name, columnName),
            IsUnique = true,
            Columns = new [] { columnName }
        };
    }
    
    private static string GetUniqueIndexName(string tableName, string columnName) {
        return $"ix_{tableName}_{columnName}_unique";
    }
}
