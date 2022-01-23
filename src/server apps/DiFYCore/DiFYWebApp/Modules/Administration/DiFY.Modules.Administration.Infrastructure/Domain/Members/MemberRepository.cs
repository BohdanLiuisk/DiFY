using System.Threading.Tasks;
using DiFY.Modules.Administration.Domain.Members;
using DiFY.Modules.Administration.Domain.Members.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiFY.Modules.Administration.Infrastructure.Domain.Members
{
    internal class MemberRepository : IMemberRepository
    {
        private readonly AdministrationContext _administrationContext;

        public MemberRepository(AdministrationContext administrationContext)
        {
            _administrationContext = administrationContext;
        }
        
        public async Task AddAsync(Member member)
        {
            await _administrationContext.Members.AddAsync(member);
        }

        public async Task<Member> GetByIdAsync(MemberId memberId)
        {
            return await _administrationContext.Members.FirstOrDefaultAsync(x => x.Id == memberId);
        }
    }
}