using Dify.Entity.ResultModels;

namespace Dify.Entity.Exceptions;

public class EntityStructureMigrationException(MigrationResult migrationResult) 
    : EntityStructureException($"Couldn't migrate entity structure. Error: {migrationResult.Exception?.Message}");
