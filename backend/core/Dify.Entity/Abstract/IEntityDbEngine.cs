using Dify.Entity.ResultModels;
using Dify.Entity.Structure;

namespace Dify.Entity.Abstract;

public interface IEntityDbEngine
{
    MigrationResult MigrateNewEntityStructures(IReadOnlyList<EntityStructure> entityStructures);

    MigrationResult MigrateExistingEntityStructure(EntityStructure entityStructure);
}
