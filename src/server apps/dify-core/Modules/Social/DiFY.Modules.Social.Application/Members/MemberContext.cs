using DiFY.BuildingBlocks.Application;
using DiFY.Modules.Social.Domain.Membership;
using DiFY.Modules.Social.Domain.Membership.Abstraction;

namespace DiFY.Modules.Social.Application.Members;

public class MemberContext : IMemberContext
{
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public MemberContext(IExecutionContextAccessor executionContextAccessor)
    {
        _executionContextAccessor = executionContextAccessor;
    }

    public MemberId MemberId => new(_executionContextAccessor.UserId);
}