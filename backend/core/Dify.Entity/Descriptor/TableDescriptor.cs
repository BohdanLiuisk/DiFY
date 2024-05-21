using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dify.Entity.Descriptor;

public class TableDescriptor
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("caption")]
    public string Caption { get; set; }

    [JsonPropertyName("columns")]
    public List<ColumnDescriptor> Columns { get; set; }

    public static TableDescriptor? DeserializeFrom(string json) {
        var tableDescriptor = JsonSerializer.Deserialize<TableDescriptor>(json);
        if (tableDescriptor == null) return null;
        foreach (var tableColumn in tableDescriptor.Columns) {
            tableColumn.Table = tableDescriptor;
        }
        return tableDescriptor;
    }
}
