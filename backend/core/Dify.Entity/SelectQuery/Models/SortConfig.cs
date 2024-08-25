using System.Text.Json.Serialization;

namespace Dify.Entity.SelectQuery.Models;

public class SortConfig
{
    [JsonPropertyName("orderPosition")]
    public int OrderPosition { get; set; }

    [JsonRequired]
    [JsonPropertyName("orderBy")]
    public string OrderBy { get; set; } = null!;

    [JsonRequired]
    [JsonPropertyName("orderDirection")]
    public string OrderDirection { get; set; } = null!;
}
