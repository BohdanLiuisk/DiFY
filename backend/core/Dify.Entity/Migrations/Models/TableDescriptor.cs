using System.Text.Json.Serialization;

namespace Dify.Entity.Migrations.Models;

public class TableDescriptor
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("code")]
    public string Code { get; set; }
    [JsonPropertyName("caption")]
    public string Caption { get; set; }
    [JsonPropertyName("columns")]
    public List<ColumnDescriptor> Columns { get; set; }
}
