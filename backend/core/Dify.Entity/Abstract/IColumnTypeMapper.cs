namespace Dify.Entity.Abstract;

public interface IColumnTypeMapper
{
    string this[ColumnType columnType] { get; }
}
