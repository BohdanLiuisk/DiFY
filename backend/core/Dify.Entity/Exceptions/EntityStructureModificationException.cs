using Dify.Entity.ResultModels;

namespace Dify.Entity.Exceptions;

public class EntityStructureModificationException : EntityStructureException
{
    public EntityStructureModificationException(IList<ModifyEntityStructureResult> modifyEntityResults)
        : base("Couldn't modify entity structures. See errors in json.", SerializeResults(modifyEntityResults)) { }
    
    public EntityStructureModificationException(ModifyEntityStructureResult modifyEntityResult)
        : base("Couldn't modify entity structure. See errors in json.", SerializeResult(modifyEntityResult)) { }
}
