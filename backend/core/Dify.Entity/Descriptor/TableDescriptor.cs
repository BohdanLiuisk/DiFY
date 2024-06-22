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
    public string Name { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("caption")]
    public string Caption { get; set; }
    
    [JsonPropertyName("columns")]
    public List<ColumnDescriptor> Columns { get; set; }

    public static TableDescriptor? DeserializeFrom(string json) {
        var tableDescriptor = JsonSerializer.Deserialize<TableDescriptor>(json);
        return tableDescriptor;
    }
}
