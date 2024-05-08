using Dify.Entity.Abstract;

namespace Dify.Entity.DbEngine.ColumnTypeMapping;

public class NpgsqlColumnTypeMapper : IColumnTypeMapper
{
    public IReadOnlyDictionary<ColumnType, string> Mapping => new Dictionary<ColumnType, string>
    {
        [ColumnType.Bigint] = "bigint"
    };

    public string this[ColumnType columnType] => Mapping[columnType];
}
