namespace Dify.Entity.Models;

public class EntityDescriptor
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Caption { get; set; }
    public string ColumnsJson { get; set; }
}
