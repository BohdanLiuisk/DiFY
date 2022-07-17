using System.Threading.Tasks;
using DiFY.Modules.Social.Domain.Calling;
using DiFY.Modules.Social.Domain.Calling.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DiFY.Modules.Social.Infrastructure.Domain.Calling;

public class CallRepository : ICallRepository
{
    private readonly SocialContext _socialContext;

    public CallRepository(SocialContext socialContext)
    {
        _socialContext = socialContext;
    }
    
    public async Task AddAsync(Call call)
    {
        await _socialContext.Calls.AddAsync(call);
    }

    public async Task<Call> GetByIdAsync(CallId callId)
    {
        return await _socialContext.Calls.FirstOrDefaultAsync(c => c.Id == callId);
    }
}
