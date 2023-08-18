using Dify.Common.Dto.Call;

namespace Dify.Core.Application.Services;

public interface IDifyNotificationService
{
    Task SendIncomingCallEventAsync(IncomingCallEventDto incomingCallEventDto, string connectionId);
}
