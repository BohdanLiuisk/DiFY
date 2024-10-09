using System.Text.Json.Serialization;

namespace Dify.Entity.SelectQuery.Models;

public class SelectQueryConfig
{
    [JsonPropertyName("entityName")] 
    [JsonRequired]
    public string EntityName { get; set; } = null!;

    [JsonPropertyName("expressions")]
    public List<SelectExpression> Expressions { get; set; } = new();
    
    [JsonPropertyName("relatedEntities")]
    public List<RelatedEntityConfig> RelatedEntities { get; set; } = new();
    
    [JsonPropertyName("filter")]
    public SelectFilter Filter { get; set; } = new();

    [JsonPropertyName("sorting")]
    public List<SortConfig>? Sorting { get; set; } = new();
    
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }
    
    [JsonPropertyName("paginationConfig")]
    public PaginationRequest? PaginationConfig { get; set; }
    
    [JsonPropertyName("debug")]
    public bool? Debug { get; set; }

    public bool IsPaginated => PaginationConfig != null && PaginationConfig.Page != 0 && PaginationConfig.PerPage != 0;
    
    public SelectExpression? FindExpressionByPath(string fullPath) {
        return Expressions
            .Select(expression => FindInExpression(expression, fullPath))
            .FirstOrDefault(found => found != null);
    }

    private SelectExpression? FindInExpression(SelectExpression expression, string fullPath) {
        if (expression.GetFullPath() == fullPath) {
            return expression;
        }
        return expression.Columns
            .Select(childExpression => FindInExpression(childExpression, fullPath))
            .FirstOrDefault(found => found != null);
    }
}
