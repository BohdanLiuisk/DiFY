using System.Data;
using Dify.Entity.Abstract;
using Dify.Entity.Structure;
using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Model;
using Microsoft.Extensions.Logging;

namespace Dify.Entity.DbEngine;

public class EntityDbEngine(ILogger<EntityDbEngine> logger, IMigrationProcessor migrationProcessor) : IEntityDbEngine 
{
    public void CreateTablesFromEntityStructures(IEnumerable<EntityStructure> entityStructures) {
        var createTableExpressions = new List<CreateTableExpression>();
        var createFkExpressions = new List<CreateForeignKeyExpression>();
        var createIndexesExpressions = new List<CreateIndexExpression>();
        foreach (var entityStructure in entityStructures) {
            var expression = GetCreateTableExpression(entityStructure);
            createTableExpressions.Add(expression);
            foreach (var foreignKey in entityStructure.ForeignKeys) {
                var createFkExpression = GetCreateForeignKeyExpression(foreignKey);
                createFkExpressions.Add(createFkExpression);
            }
            foreach (var indexStructure in entityStructure.Indexes) {
                var createIndexExpression = GetCreateUniqueIndexExpression(indexStructure);
                createIndexesExpressions.Add(createIndexExpression);
            }
        }
        migrationProcessor.BeginTransaction();
        try {
            foreach (var createTableExpression in createTableExpressions) {
                migrationProcessor.Process(createTableExpression);
            }
            foreach (var foreignKeyExpression in createFkExpressions) {
                migrationProcessor.Process(foreignKeyExpression);
            }
            foreach (var createIndexExpression in createIndexesExpressions) {
                migrationProcessor.Process(createIndexExpression);
            }
            migrationProcessor.CommitTransaction();
        } catch (Exception e) {
            migrationProcessor.RollbackTransaction();
            logger.LogError("New entities creation failed. Message: {0}", e.Message);
            throw;
        }
    }

    private CreateTableExpression GetCreateTableExpression(EntityStructure entityStructure) {
        var tableName = entityStructure.Name;
        var rootExpression = new CreateTableExpression {
            TableName = tableName
        };
        var columns = new List<ColumnDefinition>();
        foreach (var column in entityStructure.Columns) {
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
