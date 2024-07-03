using FluentMigrator.Expressions;

namespace Dify.Entity.DbEngine;

public class EntityStructureOperations
{
    public CreateTableExpression CreateTableExpression { get; set; } = null!;

    public IList<CreateForeignKeyExpression> CreateForeignKeyExpressions { get; } = 
        new List<CreateForeignKeyExpression>();

    public IList<CreateIndexExpression> CreateIndexExpressions { get; set; } = new List<CreateIndexExpression>();

    public DeleteColumnExpression? DeleteColumnsExpression { get; set; }

    public IList<AlterColumnExpression> AlterColumnExpressions { get; set; } = new List<AlterColumnExpression>();
}
