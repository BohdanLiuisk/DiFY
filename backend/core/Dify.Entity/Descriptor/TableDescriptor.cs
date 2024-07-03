using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dify.Entity.Descriptor;

public class TableDescriptor
{
    [JsonRequired]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonRequired]
    [JsonPropertyName("caption")]
    public string Caption { get; set; } = null!;
    
    [JsonPropertyName("columns")]
    public List<ColumnDescriptor> Columns { get; set; } = null!;

    public static TableDescriptor? DeserializeFrom(string json) {
        var tableDescriptor = JsonSerializer.Deserialize<TableDescriptor>(json);
        return tableDescriptor;
    }
}
