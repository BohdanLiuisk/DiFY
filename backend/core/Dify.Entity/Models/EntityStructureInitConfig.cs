using Dify.Entity.Migrations.Models;

namespace Dify.Entity.Models;

public class EntityStructureInitConfig
{
    public IList<TableDescriptor>? OuterTables { get; set; }
}
