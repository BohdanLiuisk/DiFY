using System.Text.Json.Serialization;

namespace Dify.Entity.Migrations.Models;

public class ForeignTableDescriptor
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("code")]
    public string Code { get; set; }
    [JsonPropertyName("primaryColumn")]
    public string PrimaryColumn { get; set; }
}
