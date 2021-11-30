using DiFY.BuildingBlocks.Domain.Exceptions;

namespace DiFY.BuildingBlocks.Domain
{
    public abstract class Entity
    {
        protected void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}