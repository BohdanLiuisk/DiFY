using System.Threading.Tasks;

namespace DiFY.Modules.Social.Domain.Calling.Abstractions;

public interface ICallRepository
{
    Task AddAsync(Call call);

    Task<Call> GetByIdAsync(CallId callId);
}