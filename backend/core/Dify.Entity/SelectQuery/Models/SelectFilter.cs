using System.Text.Json.Serialization;
using Dify.Entity.SelectQuery.Enums;

namespace Dify.Entity.SelectQuery.Models;

public class SelectFilter
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "clause";
    
    [JsonPropertyName("logical")]
    public string Logical { get; set; } = "and";
    
    [JsonPropertyName("path")]
    public string? Path { get; set; }
    
    [JsonPropertyName("items")]
    public List<SelectFilter> Items { get; set; } = new();

    [JsonPropertyName("predicates")]
    public List<FilterPredicate> Predicates { get; set; } = new();
    
    [JsonPropertyName("subPredicate")]
    public FilterPredicate? SubPredicate { get; set; }
    
    [JsonPropertyName("subFilter")]
    public SelectFilter? SubFilter { get; set; }
}
