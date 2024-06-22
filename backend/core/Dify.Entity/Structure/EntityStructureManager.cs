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
        var existingStructures = await GetAllExistingStructures();
        if (initConfig is { OuterTables: not null }) {
            var newTables = initConfig.OuterTables.Where(outerTable => existingStructures
                .All(table => table.Id != outerTable.Id)).ToList();
            var createResults = (await CreateEntityStructures(newTables)).ToList();
            if (createResults.Any(c => !c.Success)) {
                var invalidResultsJson = JsonSerializer.Serialize(createResults);
                throw new AggregateException($"Couldn't build entity structure. " +
                                             $"See errors in json \n {invalidResultsJson}");
            }
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
        var existingStructures = await GetAllExistingStructures();
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

    private EntityStructure CreateEntityStructureFromDescriptor(TableDescriptor tableDescriptor) {
        var entityStructure = new EntityStructure(tableDescriptor.Id, tableDescriptor.Name, tableDescriptor.Caption);
        foreach (var column in tableDescriptor.Columns) {
            var columnStructure = new EntityColumnStructure(entityStructure, column);
            if (column.ReferenceEntityId != null) {
                var foreignKey = new EntityForeignKeyStructure(entityStructure, columnStructure,
                    column.ReferenceEntityId.Value);
                columnStructure.SetForeignKey(foreignKey);
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
                foreignKeyColumn.ForeignKeyStructure.SetReferenceEntity(referenceEntity);
                entityStructure.AddForeignKey(foreignKeyColumn.ForeignKeyStructure);
            } else {
                throw new InvalidOperationException($"Couldn't find reference entity {referenceEntityId}.");
            }
        }
    }

    private async Task<IList<EntityStructure>> GetAllExistingStructures() {
        return await dbContext.EntityStructures.AsNoTracking().ToListAsync();
    }
}
