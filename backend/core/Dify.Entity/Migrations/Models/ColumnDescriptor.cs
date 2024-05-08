using System.Text.Json.Serialization;

namespace Dify.Entity.Migrations.Models;

public class ColumnDescriptor
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("code")]
    public string Code { get; set; }
    [JsonPropertyName("caption")]
    public string Caption { get; set; }
    [JsonPropertyName("type")]
    public int Type { get; set; }
    [JsonPropertyName("isPrimaryKey")]
    public bool IsPrimaryKey { get; set; }
    [JsonPropertyName("nullable")]
    public bool Nullable { get; set; }
    [JsonPropertyName("foreignTable")]
    public ForeignTableDescriptor ForeignTable { get; set; }
}
