using System.Text.Json.Serialization;
using Dify.Entity.Structure;

namespace Dify.Entity.ResultModels;

public class ModifyEntityStructureResult() : EntityStructureActionResult(EntityStructureAction.Modify)
{
    public ModifyEntityStructureResult(EntityStructure entityStructure) : this() {
        ResultStructure = entityStructure;
        EntityName = entityStructure.Name;
    }
    
    [JsonPropertyName("changed")]
    public bool IsChanged { get; set; }

    [JsonPropertyName("changedElements")]
    public IEnumerable<ChangedStructureElement>? ChangedElements { get; set; }
}
