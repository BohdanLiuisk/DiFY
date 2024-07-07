using Dify.Entity.Structure;

namespace Dify.Entity.ResultModels;

public class AlterColumnResult(EntityColumnStructure columnStructure, bool isChanged = false)
{
    private readonly IList<EntityStructureError> _errors = new List<EntityStructureError>();

    public bool IsSuccess { get; private set; } = true;
    
    public bool IsChanged { get; } = isChanged;

    public IEnumerable<EntityStructureError> Errors => _errors;

    internal void AddError(string error) {
        IsSuccess = false;
        _errors.Add(new EntityStructureError {
            ElementName = columnStructure.Name,
            ElementType = EntityStructureElementType.Column,
            Error = error
        });
    }
}
