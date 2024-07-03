using System.Data;
using Dify.Entity.Abstract;
using Dify.Entity.ResultModels;
using Dify.Entity.Structure;
using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Model;
using Microsoft.Extensions.Logging;

namespace Dify.Entity.DbEngine;

public class EntityDbEngine(ILogger<EntityDbEngine> logger, IMigrationProcessor migrationProcessor) : IEntityDbEngine 
{
    public MigrationResult MigrateNewEntityStructures(IReadOnlyList<EntityStructure> entityStructures) {
        var operations = entityStructures.Select(GetEntityStructureOperations).ToList();
        migrationProcessor.BeginTransaction();
        try {
            foreach (var entityStructure in entityStructures) {
                migrationProcessor.Process(new DeleteTableExpression {
                    TableName = entityStructure.Name,
                    IfExists = true
                });
            }
            foreach (var dbOperations in operations) {
                migrationProcessor.Process(dbOperations.CreateTableExpression);
            }
            foreach (var dbOperations in operations) {
                foreach (var foreignKeyExpression in dbOperations.CreateForeignKeyExpressions) {
                    migrationProcessor.Process(foreignKeyExpression);
                }
                foreach (var createIndexExpression in dbOperations.CreateIndexExpressions) {
                    migrationProcessor.Process(createIndexExpression);
                }
            }
            migrationProcessor.CommitTransaction();
        } catch (Exception e) {
            migrationProcessor.RollbackTransaction();
            logger.LogError("New entities creation failed. {0}", e.Message);
            return new MigrationResult(isSuccess: false, e);
        }
        return new MigrationResult(isSuccess: true);
    }

    public MigrationResult MigrateExistingEntityStructure(EntityStructure entityStructure) {
        var operations = GetEntityStructureOperations(entityStructure);
        migrationProcessor.BeginTransaction();
        try {
            if (operations.DeleteColumnsExpression != null) {
                migrationProcessor.Process(operations.DeleteColumnsExpression);
            }
            foreach (var createColumnExpression in operations.CreateColumnExpressions) {
                migrationProcessor.Process(createColumnExpression);
            }
            foreach (var foreignKeyExpression in operations.CreateForeignKeyExpressions) {
                migrationProcessor.Process(foreignKeyExpression);
            }
            foreach (var createIndexExpression in operations.CreateIndexExpressions) {
                migrationProcessor.Process(createIndexExpression);
            }
            migrationProcessor.CommitTransaction();
        } catch (Exception e) {
            migrationProcessor.RollbackTransaction();
            logger.LogError("Entity structure migration failed. {0}", e.Message);
            return new MigrationResult(isSuccess: false, e);
        }
        return new MigrationResult(isSuccess: true);
    }

    private EntityStructureOperations GetEntityStructureOperations(EntityStructure entityStructure) {
        var operations = new EntityStructureOperations();
        var createTableExpression = GetCreateTableExpression(entityStructure);
        operations.CreateTableExpression = createTableExpression;
        var newColumns = entityStructure.Columns.Where(c => c.State == EntityStructureElementState.New);
        foreach (var newColumn in newColumns) {
            var createColumnExpression = GetCreateColumnExpression(newColumn);
            operations.CreateColumnExpressions.Add(createColumnExpression);
        }
        var newForeignKeys = entityStructure.ForeignKeys.Where(f => f.State == EntityStructureElementState.New);
        foreach (var foreignKey in newForeignKeys) {
            var createFkExpression = GetCreateForeignKeyExpression(foreignKey);
            operations.CreateForeignKeyExpressions.Add(createFkExpression);
        }
        var newIndexes = entityStructure.Indexes.Where(i => i.State == EntityStructureElementState.New);
        foreach (var indexStructure in newIndexes) {
            var createIndexExpression = GetCreateUniqueIndexExpression(indexStructure);
            operations.CreateIndexExpressions.Add(createIndexExpression);
        }
        var deletedColumns = entityStructure.Columns
            .Where(c => c.State == EntityStructureElementState.Deleted)
            .Select(c => c.DbName)
            .ToList();
        if (deletedColumns.Count != 0) {
            operations.DeleteColumnsExpression = new DeleteColumnExpression {
                TableName = entityStructure.Name,
                ColumnNames = deletedColumns
            };
        }
        return operations;
    }

    private CreateTableExpression GetCreateTableExpression(EntityStructure entityStructure) {
        var tableName = entityStructure.Name;
        var rootExpression = new CreateTableExpression {
            TableName = tableName
        };
        var columns = new List<ColumnDefinition>();
        var newColumns = entityStructure.Columns.Where(c => c.State == EntityStructureElementState.New);
        foreach (var column in newColumns) {
            var columnDefinition = GenerateColumnDefinition(column);
            columns.Add(columnDefinition);
        }
        rootExpression.Columns = columns;
        return rootExpression;
    }
    
    private ColumnDefinition GenerateColumnDefinition(EntityColumnStructure columnStructure) {
        var columnDefinition = new ColumnDefinition {
            Name = columnStructure.DbName,
            TableName = columnStructure.EntityStructure.Name,
            ModificationType = ColumnModificationType.Create,
            Type = columnStructure.Type,
            IsPrimaryKey = columnStructure.IsPrimaryKey,
            Size = columnStructure.Size,
            Precision = columnStructure.Precision,
            IsNullable = columnStructure.IsNullable,
            IsUnique = columnStructure.IsUnique,
            IsForeignKey = columnStructure.IsForeignKey
        };
        if (columnDefinition.IsPrimaryKey) {
            switch (columnDefinition.Type) {
                case DbType.Int32:
                    columnDefinition.IsIdentity = true;
                    break;
                case DbType.Guid:
                    columnDefinition.DefaultValue = SystemMethods.NewGuid;
                    break;
            }
        }
        return columnDefinition;
    }

    private CreateColumnExpression GetCreateColumnExpression(EntityColumnStructure columnStructure) {
        var columnDefinition = GenerateColumnDefinition(columnStructure);
        return new CreateColumnExpression {
            Column = columnDefinition,
            TableName = columnStructure.EntityStructure.Name
        };
    }
    
    private CreateForeignKeyExpression GetCreateForeignKeyExpression(EntityForeignKeyStructure foreignKeyStructure) {
        var fkDefinition = new ForeignKeyDefinition {
            Name = foreignKeyStructure.ForeignKeyName,
            ForeignTable = foreignKeyStructure.PrimaryEntityName,
            PrimaryTable = foreignKeyStructure.ReferenceEntityName,
            ForeignColumns = new List<string> { foreignKeyStructure.PrimaryColumnName },
            PrimaryColumns = new List<string> { foreignKeyStructure.ReferenceColumnName },
            OnDelete = Rule.SetDefault
        };
        return new CreateForeignKeyExpression {
            ForeignKey = fkDefinition
        };
    }

    private CreateIndexExpression GetCreateUniqueIndexExpression(EntityIndexStructure indexStructure) {
        var indexColumnDefinition = indexStructure.Columns.Select(
            columnName => new IndexColumnDefinition { Name = columnName }).ToList();
        var indexDefinition = new IndexDefinition {
            Name = indexStructure.Name,
            TableName = indexStructure.EntityStructure.Name,
            IsUnique = true,
            Columns = indexColumnDefinition
        };
        return new CreateIndexExpression {
            Index = indexDefinition
        };
    }
}
