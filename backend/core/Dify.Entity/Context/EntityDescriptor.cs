using System.Text.Json;
using Dify.Entity.Descriptor;

namespace Dify.Entity.Context;

public class EntityDescriptor
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Caption { get; set; }
    public string ColumnsJson { get; set; }

    public TableDescriptor ToTableDescriptor() {
        var tableDescriptor = new TableDescriptor {
            Id = Id,
            Name = Code,
            Caption = Caption,
        };
        var columns = JsonSerializer.Deserialize<List<ColumnDescriptor>>(ColumnsJson) ?? new List<ColumnDescriptor>();
        var deserializedColumns = columns.Select(column => {
            column.Table = tableDescriptor;
            return column;
        }).ToList();
        tableDescriptor.Columns = deserializedColumns;
        return tableDescriptor;
    }
}
