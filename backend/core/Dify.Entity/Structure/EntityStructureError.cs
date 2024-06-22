using System.Text.Json.Serialization;

namespace Dify.Entity.Structure;

public class EntityStructureError
{
    [JsonPropertyName("elementType")]
    public EntityStructureElementType ElementType { get; set; }
    
    [JsonPropertyName("elementName")]
    public required string ElementName { get; set; } 
    
    [JsonPropertyName("error")]
    public required string Error { get; set; }
}
