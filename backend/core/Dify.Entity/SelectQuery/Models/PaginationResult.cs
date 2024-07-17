using System.Text.Json.Serialization;

namespace Dify.Entity.SelectQuery.Models;

public class PaginationResult(long count, int page, int perPage, int totalPages)
{
    [JsonPropertyName("count")]
    public long Count { get; set; } = count;

    [JsonPropertyName("page")]
    public int Page { get; set; } = page;

    [JsonPropertyName("perPage")]
    public int PerPage { get; set; } = perPage;

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; } = totalPages;
}
