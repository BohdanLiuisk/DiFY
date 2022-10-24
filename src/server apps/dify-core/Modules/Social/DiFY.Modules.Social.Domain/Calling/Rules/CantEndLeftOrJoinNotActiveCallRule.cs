using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Calling.Rules;

public class CantEndLeftOrJoinNotActiveCallRule : IBusinessRule
{
    private readonly bool _isActive;

    public CantEndLeftOrJoinNotActiveCallRule(bool active)
    {
        _isActive = active;
    }

    public bool IsBroken() => !_isActive;
    
    public string Message => "This call is already over.";
}