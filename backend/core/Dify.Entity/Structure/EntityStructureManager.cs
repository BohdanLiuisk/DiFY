using System.Text.Json;
using Dify.Entity.Abstract;
using Dify.Entity.Context;
using Dify.Entity.Descriptor;
using Dify.Entity.Initialization;
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
        logger.LogInformation("initialization started");
        var existingStructures = await GetAllEntityStructures();
        if (initConfig is { OuterTables: not null }) {
            var newTables = initConfig.OuterTables.Where(outerTable => existingStructures
                .All(table => table.Id != outerTable.Id)).ToList();
            if (newTables.Count == 0) return;
            var createResults = (await CreateEntityStructures(newTables)).ToList();
            if (createResults.Any(c => !c.Success)) {
                var invalidResultsJson = JsonSerializer.Serialize(createResults);
                throw new AggregateException($"Couldn't build entity structure. " +
                                             $"See errors in json \n {invalidResultsJson}");
            }
            await SaveNewStructures(createResults);
        }
    }
    
    public async Task<IEnumerable<CreateEntityResult>> CreateEntityStructures(
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
        try {
            entityDbEngine.CreateTablesFromEntityStructures(newEntityStructures);
        } catch (Exception ex) {
            var createResult = new CreateEntityResult();
            createResult.SetException(ex);
            return new [] { createResult };
        }
        return newEntityStructures.Select(structure => new CreateEntityResult {
            EntityName = structure.Name,
            ResultStructure = structure
        });
    }
    
    public async Task<IList<EntityStructure>> GetAllEntityStructures() {
        var entityStructures = await dbContext.EntityStructures.AsNoTracking().ToListAsync();
        foreach (var entityStructure in entityStructures) {
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

    private EntityStructure CreateEntityStructureFromDescriptor(TableDescriptor tableDescriptor) {
        var entityStructure = new EntityStructure(tableDescriptor.Id, tableDescriptor.Name, tableDescriptor.Caption);
        foreach (var column in tableDescriptor.Columns) {
            var columnStructure = new EntityColumnStructure(entityStructure, column);
            if (column.ReferenceEntityId != null) {
                var foreignKey = new EntityForeignKeyStructure(entityStructure, columnStructure,
                    column.ReferenceEntityId.Value);
                columnStructure.DefineForeignKey(foreignKey);
            }
            entityStructure.AddColumn(columnStructure);
        }
        return entityStructure;
    }

    private void InitEntityStructureIndexes(EntityStructure entityStructure) {
        var indexes = new List<EntityIndexStructure>();
        foreach (var columnStructure in entityStructure.Columns) {
            if (columnStructure.IsUnique) {
                var indexStructure = EntityIndexStructure.CreateUniqueIndex(entityStructure, columnStructure.Name);
                indexes.Add(indexStructure);
            }
        }
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

    private async Task SaveNewStructures(IReadOnlyCollection<CreateEntityResult> createEntityResults) {
        if(createEntityResults.Count == 0) return;
        foreach (var createResult in createEntityResults) {
            if (createResult.ResultStructure != null) {
                await dbContext.EntityStructures.AddAsync(createResult.ResultStructure);
            }
        }
        await dbContext.SaveChangesAsync();
    }

    private async Task ResolveEntityStructureReferences(EntityStructure entityStructure, 
        IList<EntityStructure>? entityStructures = null) {
        entityStructure.DeserializeProperties();
        entityStructures ??= await dbContext.EntityStructures.AsNoTracking().ToListAsync();
        foreach (var foreignKey in entityStructure.ForeignKeys) {
            var referenceEntity = entityStructures.FirstOrDefault(s => s.Id == foreignKey.ReferenceEntityId);
            if (referenceEntity == null) {
                var message = $"Couldn't find reference entity with id {foreignKey.ReferenceEntityId}";
                throw new InvalidOperationException(message);
            }
            foreignKey.ReferenceEntityStructure = referenceEntity;
        }
    }
}
