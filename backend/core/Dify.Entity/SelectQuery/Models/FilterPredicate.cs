using System.Text.Json;
using System.Text.Json.Serialization;
using Dify.Entity.SelectQuery.Enums;

namespace Dify.Entity.SelectQuery.Models;

public class FilterPredicate
{
    [JsonPropertyName("operator")]
    [JsonRequired]
    public string Operator { get; set; } = null!;
    
    [JsonPropertyName("valueType")]
    public PredicateValueType ValueType { get; set; } = PredicateValueType.Value;
    
    [JsonPropertyName("value")]
    public JsonElement? Value { get; set; }
    
    [JsonPropertyName("datePart")]
    public string? DatePart { get; set; }
}
