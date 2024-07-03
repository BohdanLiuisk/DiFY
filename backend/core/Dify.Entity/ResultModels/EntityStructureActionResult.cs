using System.Text.Json.Serialization;
using Dify.Entity.Structure;

namespace Dify.Entity.ResultModels;

public abstract class EntityStructureActionResult(EntityStructureAction action)
{
    private readonly List<EntityStructureError> _errors = new();

    [JsonPropertyName("operationType")]
    public EntityStructureAction Action { get; private set; } = action;

    [JsonPropertyName("entityName")]
    public string? EntityName { get; internal set; }
    
    [JsonPropertyName("success")]
    public bool Success { get; private set; } = true;
    
    [JsonPropertyName("errors")]
    public IEnumerable<EntityStructureError> Errors  => _errors;
    
    [JsonPropertyName("exception")]
    public string? Exception { get; protected set; }

    [JsonIgnore]
    public EntityStructure? ResultStructure { get; internal init; }
    
    internal void AddErrors(IReadOnlyList<EntityStructureError> errors) {
        if (errors.Count == 0) return;
        Success = false;
        _errors.AddRange(errors);
    }

    internal void SetException(Exception exception) {
        Success = false;
        Exception = $"{exception.Message}\n{exception.InnerException?.Message}";
    }
}
