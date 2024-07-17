using System.Text.Json.Serialization;

namespace Dify.Entity.SelectQuery.Models;

public class SelectQueryConfig
{
    [JsonPropertyName("entityName")] 
    [JsonRequired]
    public string EntityName { get; set; } = null!;

    [JsonPropertyName("columns")]
    public List<SelectColumnConfig> Columns { get; set; } = new();
    
    [JsonPropertyName("relatedEntities")]
    public List<RelatedEntityConfig> RelatedEntities { get; set; } = new();
    
    [JsonPropertyName("filters")]
    public List<FilterConfig> Filters { get; set; } = new();
    
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }
    
    [JsonPropertyName("paginationConfig")]
    public PaginationRequest? PaginationConfig { get; set; }

    public bool IsPaginated => PaginationConfig != null && PaginationConfig.Page != 0 && PaginationConfig.PerPage != 0;
}
