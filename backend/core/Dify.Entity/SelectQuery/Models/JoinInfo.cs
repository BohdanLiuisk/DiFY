using Dify.Entity.Structure;

namespace Dify.Entity.SelectQuery.Models;

public class JoinInfo(string joinPath, string alias, EntityStructure primaryStructure, 
    EntityStructure parentStructure)
{
    public string JoinPath { get; } = joinPath;

    public string Alias { get; } = alias;

    public EntityStructure PrimaryStructure { get; } = primaryStructure;

    public EntityStructure ParentStructure { get; } = parentStructure;
}
