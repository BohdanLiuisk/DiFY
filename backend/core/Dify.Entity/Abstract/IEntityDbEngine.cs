using Dify.Entity.Descriptor;
using FluentMigrator.Expressions;

namespace Dify.Entity.Abstract;

public interface IEntityDbEngine
{
    void CreateNewTables(IList<TableDescriptor> newTables);
    
    CreateTableExpression GenerateCreateTableExpression(TableDescriptor tableDescriptor);
}
