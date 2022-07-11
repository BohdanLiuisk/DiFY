using System.Threading.Tasks;

namespace DiFY.Modules.Administration.Domain.Members.Interfaces
{
    public interface IMemberRepository
    {
        Task AddAsync(Member member);

        Task<Member> GetByIdAsync(MemberId memberId);
    }
}