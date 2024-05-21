using System.Text.Json.Serialization;

namespace Dify.Entity.Descriptor;

public class ColumnDescriptor
{
    public TableDescriptor Table { get; set; }
    
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("caption")]
    public string Caption { get; set; }

    [JsonPropertyName("type")]
    public int Type { get; set; }
    
    [JsonPropertyName("size")]
    public int? Size { get; set; }

    [JsonPropertyName("isPrimaryKey")]
    public bool? IsPrimaryKey { get; set; }
    
    [JsonPropertyName("isUnique")]
    public bool? IsUnique { get; set; }

    [JsonPropertyName("isNullable")]
    public bool? IsNullable { get; set; }

    [JsonPropertyName("foreignTable")]
    public ForeignTableDescriptor? ForeignTable { get; set; }
}
