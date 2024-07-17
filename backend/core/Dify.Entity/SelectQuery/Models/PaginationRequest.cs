using System.Text.Json.Serialization;

namespace Dify.Entity.SelectQuery.Models;

public class PaginationRequest
{
    [JsonPropertyName("page")]
    public int Page { get; set; }
    
    [JsonPropertyName("perPage")]
    public int PerPage { get; set; }
}
