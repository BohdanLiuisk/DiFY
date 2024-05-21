using System.Text.Json.Serialization;

namespace Dify.Entity.Descriptor;

public class ForeignTableDescriptor
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("foreignColumn")]
    public string ForeignColumn { get; set; }
}
