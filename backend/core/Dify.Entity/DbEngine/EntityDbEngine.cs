using System.Data;
using Dify.Entity.Abstract;
using Dify.Entity.Descriptor;
using Dify.Entity.Sorting;
using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dify.Entity.DbEngine;

public class EntityDbEngine(ILogger<EntityDbEngine> logger, IServiceProvider serviceProvider) : IEntityDbEngine 
{
    public void CreateNewTables(IList<TableDescriptor> newTables) {
        var migrationProcessor = GetMigrationProcessor();
        var createTableExpressions = new List<CreateTableExpression>();
        var graph = new TableDependencyGraph(newTables);
        var sortedTables = graph.SortTables();
        foreach (var newTable in sortedTables) {
            var expression = GenerateCreateTableExpression(newTable);
            createTableExpressions.Add(expression);
        }
        migrationProcessor.BeginTransaction();
        try {
            foreach (var createTableExpression in createTableExpressions) {
                migrationProcessor.Process(createTableExpression);
                var foreignKeyExpressions = GetForeignKeyExpressionForColumns(createTableExpression.Columns);
                var uniqueIndexExpressions = GetUniqueIndexExpressionForColumns(createTableExpression.Columns);
                foreach (var foreignKeyExpression in foreignKeyExpressions) {
                    migrationProcessor.Process(foreignKeyExpression);
                }
                foreach (var uniqueIndexExpression in uniqueIndexExpressions) {
                    migrationProcessor.Process(uniqueIndexExpression);
                }
            }
            migrationProcessor.CommitTransaction();
        } catch (Exception e) {
            migrationProcessor.RollbackTransaction();
            logger.LogError("New tables creation failed. Message {0}", e.Message);
        }
    }
    
    public CreateTableExpression GenerateCreateTableExpression(TableDescriptor tableDescriptor) {
        var tableName = tableDescriptor.Name;
        var rootExpression = new CreateTableExpression {
            TableName = tableName
        };
        var columns = new List<ColumnDefinition>();
        foreach (var column in tableDescriptor.Columns) {
            var columnDefinition = GenerateColumnDefinition(column);
            columns.Add(columnDefinition);
        }
        rootExpression.Columns = columns;
        return rootExpression;
    }

    private ColumnDefinition GenerateColumnDefinition(ColumnDescriptor columnDescriptor) {
        var columnType = (DbType)columnDescriptor.Type;
        var columnDefinition = new ColumnDefinition {
            Name = columnDescriptor.Name,
            TableName = columnDescriptor.Table.Name,
            ModificationType = ColumnModificationType.Create,
            Type = columnType
        };
        if (columnDescriptor.IsPrimaryKey is true) {
            columnDefinition.IsPrimaryKey = true;
            switch (columnDefinition.Type) {
                case DbType.Int32:
                    columnDefinition.IsIdentity = true;
                    break;
                case DbType.Guid:
                    columnDefinition.DefaultValue = SystemMethods.NewGuid;
                    break;
            }
        }
        if (GetSizePropertyApplicable(columnType) && columnDescriptor.Size.HasValue) {
            columnDefinition.Size = columnDescriptor.Size.Value;
        }
        if (columnDescriptor.IsUnique != null) {
            columnDefinition.IsUnique = columnDescriptor.IsUnique.Value;
        }
        if (columnDescriptor.IsNullable != null) {
            columnDefinition.IsNullable = columnDescriptor.IsNullable.Value;
        }
        if (columnDescriptor.ForeignTable == null) return columnDefinition;
        columnDefinition.IsForeignKey = true;
        var fkName = GetForeignKeyName(columnDescriptor);
        columnDefinition.ForeignKey = new ForeignKeyDefinition {
            Name = fkName,
            ForeignTable = columnDescriptor.Table.Name,
            PrimaryTable = columnDescriptor.ForeignTable.Name,
            ForeignColumns = new List<string> { columnDescriptor.Name },
            PrimaryColumns = new List<string> { columnDescriptor.ForeignTable.ForeignColumn },
            OnDelete = Rule.SetDefault
        };
        return columnDefinition;
    }
    
    private IEnumerable<CreateForeignKeyExpression> GetForeignKeyExpressionForColumns(
        IEnumerable<ColumnDefinition> columnDefinitions) {
        return columnDefinitions.Where(c => c is { IsForeignKey: true, ForeignKey: not null }).Select(c => 
            new CreateForeignKeyExpression {
                ForeignKey = c.ForeignKey
            }
        );
    }

    private IEnumerable<CreateIndexExpression> GetUniqueIndexExpressionForColumns(
        IEnumerable<ColumnDefinition> columnDefinitions) {
        return columnDefinitions.Where(c => c.IsUnique).Select(columnDefinition => new CreateIndexExpression {
            Index = new IndexDefinition {
                Name = GetUniqueIndexName(columnDefinition.TableName, columnDefinition.Name),
                TableName = columnDefinition.TableName,
                IsUnique = true,
                Columns = new List<IndexColumnDefinition> {
                    new() { Name = columnDefinition.Name }
                }
            } 
        });
    }

    private string GetForeignKeyName(ColumnDescriptor column) {
        return $"fk_{column.Table.Name}_{column.ForeignTable?.Name}_{column.Name}";
    }

    private string GetUniqueIndexName(string tableName, string columnName) {
        return $"ix_{tableName}_{columnName}_unique";
    } 
    
    private bool GetSizePropertyApplicable(DbType dbType) {
        return dbType is DbType.AnsiString or DbType.Binary or DbType.String 
            or DbType.VarNumeric or DbType.AnsiStringFixedLength or DbType.StringFixedLength;
    }

    private IMigrationProcessor GetMigrationProcessor() {
        using var scope = serviceProvider.CreateScope();
        var migrationProcessor = scope.ServiceProvider.GetService<IMigrationProcessor>();
        if (migrationProcessor == null) {
            throw new AggregateException("Migration processor is not available.");
        }
        return migrationProcessor;
    }
}
