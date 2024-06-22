using System.Text.Json.Serialization;

namespace Dify.Entity.Structure;

public class CreateEntityResult
{
    private readonly List<EntityStructureError> _errors = new();
    
    [JsonPropertyName("entityName")]
    public string? EntityName { get; init; }
    
    [JsonPropertyName("success")]
    public bool Success { get; private set; } = true;
    
    [JsonPropertyName("errors")]
    public IEnumerable<EntityStructureError> Errors  => _errors;
    
    [JsonPropertyName("exception")]
    public string? Exception { get; private set; }
    
    [JsonIgnore]
    public EntityStructure? ResultStructure { get; set; }
    
    internal void AddErrors(IEnumerable<EntityStructureError> errors) {
        var errorsList = errors.ToList();
        if (errorsList.Count == 0) return;
        Success = false;
        _errors.AddRange(errorsList);
    }

    internal void SetException(Exception exception) {
        Success = false;
        Exception = $"{exception.Message}\n{exception.InnerException?.Message}";
    }
}
