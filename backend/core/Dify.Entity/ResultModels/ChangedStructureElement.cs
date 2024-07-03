using Dify.Entity.Structure;

namespace Dify.Entity.ResultModels;

public class ChangedStructureElement
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required EntityStructureAction Action { get; set; }
}
