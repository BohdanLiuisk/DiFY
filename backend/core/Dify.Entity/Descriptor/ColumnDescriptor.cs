using System.Text.Json.Serialization;

namespace Dify.Entity.Descriptor;

public class ColumnDescriptor
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("caption")]
    public string Caption { get; set; } = null!;

    [JsonPropertyName("type")]
    public int Type { get; set; }
    
    [JsonPropertyName("size")]
    public int? Size { get; set; }
    
    [JsonPropertyName("precision")]
    public int? Precision { get; set; }

    [JsonPropertyName("isPrimaryKey")]
    public bool? IsPrimaryKey { get; set; }
    
    [JsonPropertyName("isUnique")]
    public bool? IsUnique { get; set; }

    [JsonPropertyName("isNullable")]
    public bool? IsNullable { get; set; }
    
    [JsonPropertyName("referenceEntityId")]
    public Guid? ReferenceEntityId { get; set; }
}
