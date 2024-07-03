using Dify.Entity.Abstract;
using Dify.Entity.Context;
using Dify.Entity.Descriptor;
using Dify.Entity.Exceptions;
using Dify.Entity.Initialization;
using Dify.Entity.ResultModels;
using Dify.Entity.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dify.Entity.Structure;

public class EntityStructureManager(
    DifyEntityContext dbContext, 
    IEntityDbEngine entityDbEngine,
    ILogger<EntityStructureManager> logger)
{
    internal async Task InitializeAsync(EntityStructureInitConfig? initConfig) {
        logger.LogInformation("Initialization started.");
        if (initConfig is { OuterTables: not null }) {
            var existingStructures = await GetAllEntityStructures();
            var newTables = initConfig.OuterTables.Where(outerTable => existingStructures
                .All(table => table.Id != outerTable.Id)).ToList();
            if (newTables.Count >= 1) {
                var createResults = (await CreateEntityStructures(newTables)).ToList();
                if (createResults.Any(c => !c.Success)) {
                    throw new EntityStructureCreationException(createResults);
                }
                await SaveNewStructures(createResults);
            }
            var existingTables = initConfig.OuterTables.Where(outerTable => existingStructures
                .Any(table => table.Id == outerTable.Id)).ToList();
            var outerChanges = (await GetModifiedEntitiesChanges(existingTables)).ToList();
            if (outerChanges.Any(c => !c.Success)) {
                throw new EntityStructureModificationException(outerChanges);
            }
            foreach (var changeEntityResult in outerChanges) {
                if (changeEntityResult is { IsChanged: true, ResultStructure: not null }) {
                    var migrationResult = entityDbEngine.MigrateExistingEntityStructure(
                        changeEntityResult.ResultStructure);
                    if (!migrationResult.IsSuccess) {
                        throw new EntityStructureMigrationException(migrationResult);
                    }
                    await SaveEntityStructure(changeEntityResult.ResultStructure);
                }
            }
        }
    }
    
    public async Task<IEnumerable<CreateEntityStructureResult>> CreateEntityStructures(
        IEnumerable<TableDescriptor> tableDescriptors) {
        var tableDescriptorsList = tableDescriptors.ToList();
        var invalidTables = tableDescriptorsList.Select(EntityStructureValidator.ValidateTableDescriptor)
            .Where(v => !v.Success).ToList();
        if (invalidTables.Count != 0) {
            return invalidTables.Select(r => r.ToCreateEntityResult());
        }
        var newEntityStructures = new List<EntityStructure>();
        foreach (var tableDescriptor in tableDescriptorsList) {
            var newEntityStructure = CreateEntityStructureFromDescriptor(tableDescriptor);
            newEntityStructures.Add(newEntityStructure);
        }
        var existingStructures = await GetAllEntityStructures();
        var allStructures = newEntityStructures.Union(existingStructures).ToList();
        foreach (var newStructure in newEntityStructures) {
            InitEntityRelations(newStructure, allStructures);
        }
        foreach (var newEntityStructure in newEntityStructures) {
            InitEntityStructureIndexes(newEntityStructure);
        }
        var invalidStructures = newEntityStructures.Select(entityStructure => EntityStructureValidator
            .ValidateEntityStructure(entityStructure, allStructures)).Where(r => !r.Success).ToList();
        if (invalidStructures.Count != 0) {
            return invalidStructures.Select(r => r.ToCreateEntityResult());
        }
        var migrationResult = entityDbEngine.MigrateNewEntityStructures(newEntityStructures);
        if (!migrationResult.IsSuccess) {
            return new [] { new CreateEntityStructureResult(migrationResult.Exception) };
        }
        return newEntityStructures.Select(structure => new CreateEntityStructureResult(structure));
    }
    
    public async Task<IList<EntityStructure>> GetAllEntityStructures() {
        var entityStructures = await dbContext.EntityStructures.AsNoTracking().ToListAsync();
        foreach (var entityStructure in entityStructures) {
            entityStructure.DeserializeProperties();
            await ResolveEntityStructureReferences(entityStructure, entityStructures);
        }
        return entityStructures;
    }

    public async Task<EntityStructure> FindEntityStructureById(Guid id) {
        var entityStructures = await GetAllEntityStructures();
        return entityStructures.First(es => es.Id == id);
    }

    public async Task<EntityStructure> FindEntityStructureByName(string name) {
        var entityStructures = await GetAllEntityStructures();
        return entityStructures.First(es => es.Name == name);
    }

    private async Task<IEnumerable<ModifyEntityStructureResult>> GetModifiedEntitiesChanges(
        IReadOnlyList<TableDescriptor> tableDescriptors) {
        var existingStructures = await GetAllEntityStructures();
        var invalidTables = tableDescriptors.Select(EntityStructureValidator.ValidateTableDescriptor)
            .Where(v => !v.Success).ToList();
        if (invalidTables.Count != 0) {
            return invalidTables.Select(r => r.ToModifyEntityResult());
        }
        var changeEntityResults = new List<ModifyEntityStructureResult>();
        foreach (var tableDescriptor in tableDescriptors) {
            var entityStructure = existingStructures.FirstOrDefault(es => es.Id == tableDescriptor.Id);
            if (entityStructure == null) continue;
            var newColumnIds = tableDescriptor.Columns.Select(c => c.Id).ToHashSet();
            var originalColumnIds = entityStructure.Columns.Select(c => c.Id).ToHashSet();
            var deletedColumns = entityStructure.Columns.Where(column => !newColumnIds.Contains(column.Id)).ToList();
            var newColumns = tableDescriptor.Columns.Where(column => !originalColumnIds.Contains(column.Id)).ToList();
            foreach (var newColumnDescriptor in newColumns) {
                entityStructure.AddColumn(newColumnDescriptor);
            }
            foreach (var deletedColumn in deletedColumns) {
                entityStructure.DeleteColumnById(deletedColumn.Id);
            }
            var existingColumns = tableDescriptor.Columns.Where(item => originalColumnIds.Contains(item.Id)).ToList();
            foreach (var column in existingColumns) {
                var columnDescriptor = entityStructure.Columns.FirstOrDefault(c => c.Id == column.Id);
            }
            var validationResult = EntityStructureValidator.ValidateEntityStructure(
                entityStructure, existingStructures);
            if (!validationResult.Success) {
                changeEntityResults.Add(validationResult.ToModifyEntityResult());
                continue;
            }
            if (entityStructure.IsChanged) {
                changeEntityResults.Add(new ModifyEntityStructureResult(entityStructure));
            }
        }
        return changeEntityResults;
    }

    private EntityStructure CreateEntityStructureFromDescriptor(TableDescriptor tableDescriptor) {
        var entityStructure = new EntityStructure(tableDescriptor.Id, tableDescriptor.Name, tableDescriptor.Caption);
        foreach (var columnDescriptor in tableDescriptor.Columns) {
            entityStructure.AddColumn(columnDescriptor);
        }
        return entityStructure;
    }
    
    private void InitEntityStructureIndexes(EntityStructure entityStructure) {
        var indexes = entityStructure.Columns
            .Where(column => column.IsUnique)
            .Select(column => EntityIndexStructure.CreateUniqueIndex(entityStructure, column.Name))
            .ToList();
        if (indexes.Count == 0) return;
        foreach (var index in indexes) {
            entityStructure.AddIndex(index);
        }
    }

    private void InitEntityRelations(EntityStructure entityStructure, IList<EntityStructure> allStructures) {
        var foreignKeyColumns = entityStructure.Columns.Where(
            c => c is { IsForeignKey: true, ForeignKeyStructure: not null }).ToList();
        foreach (var foreignKeyColumn in foreignKeyColumns) {
            if (foreignKeyColumn.ForeignKeyStructure == null) continue;
            var referenceEntityId = foreignKeyColumn.ForeignKeyStructure.ReferenceEntityId;
            var referenceEntity = allStructures.FirstOrDefault(s => s.Id == referenceEntityId);
            if (referenceEntity != null) {
                foreignKeyColumn.ForeignKeyStructure.LinkReferenceEntity(referenceEntity);
                entityStructure.AddForeignKey(foreignKeyColumn.ForeignKeyStructure);
            } else {
                throw new InvalidOperationException($"Couldn't find reference entity {referenceEntityId}.");
            }
        }
    }

    private async Task SaveNewStructures(IReadOnlyCollection<CreateEntityStructureResult> createEntityResults) {
        if(createEntityResults.Count == 0) return;
        foreach (var createResult in createEntityResults) {
            if (createResult.ResultStructure == null) continue;
            createResult.ResultStructure.BeforeSaved();
            await dbContext.EntityStructures.AddAsync(createResult.ResultStructure);
        }
        await dbContext.SaveChangesAsync();
    }

    private async Task SaveEntityStructure(EntityStructure entityStructure) {
        entityStructure.BeforeSaved();
        dbContext.EntityStructures.Update(entityStructure);
        await dbContext.SaveChangesAsync();
    }

    private async Task ResolveEntityStructureReferences(EntityStructure entityStructure, 
        IList<EntityStructure>? allStructures = null) {
        allStructures ??= await dbContext.EntityStructures.AsNoTracking().ToListAsync();
        foreach (var foreignKey in entityStructure.ForeignKeys) {
            var referenceEntity = allStructures.FirstOrDefault(s => s.Id == foreignKey.ReferenceEntityId);
            if (referenceEntity == null) {
                var message = $"Couldn't find reference entity with id {foreignKey.ReferenceEntityId}";
                throw new InvalidOperationException(message);
            }
            foreignKey.ReferenceEntityStructure = referenceEntity;
        }
    }
}
