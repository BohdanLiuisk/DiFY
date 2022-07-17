using System.Threading.Tasks;

namespace DiFY.Modules.Social.Domain.Membership.Abstraction;

public interface IMemberRepository
{
    Task AddAsync(Member member);

    Task<Member> GetByIdAsync(MemberId memberId);
}