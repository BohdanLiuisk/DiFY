using System.Threading.Tasks;

namespace DiFY.Modules.Social.Domain.Calling.Abstractions;

public interface ICallParticipantRepository
{
    Task<CallParticipant> GetByIdAsync(CallParticipantId callParticipantId);
}