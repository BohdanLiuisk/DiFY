using Dify.Entity.Descriptor;

namespace Dify.Entity.Initialization;

public class EntityStructureInitConfig
{
    public IList<TableDescriptor>? OuterTables { get; init; }
}
