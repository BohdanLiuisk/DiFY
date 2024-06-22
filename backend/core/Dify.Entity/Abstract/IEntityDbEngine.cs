using Dify.Entity.Structure;

namespace Dify.Entity.Abstract;

public interface IEntityDbEngine
{
    void CreateTablesFromEntityStructures(IEnumerable<EntityStructure> entityStructures);
}
