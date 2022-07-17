using System.Threading.Tasks;
using DiFY.Modules.Social.Domain.Membership;
using DiFY.Modules.Social.Domain.Membership.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DiFY.Modules.Social.Infrastructure.Domain.Membership;

public class MemberRepository : IMemberRepository
{
    private readonly SocialContext _socialContext;

    public MemberRepository(SocialContext socialContext)
    {
        _socialContext = socialContext;
    }
    
    public async Task AddAsync(Member member)
    {
        await _socialContext.Members.AddAsync(member);
    }

    public async Task<Member> GetByIdAsync(MemberId memberId)
    {
        return await _socialContext.Members.FirstOrDefaultAsync(x => x.Id == memberId);
    }
}