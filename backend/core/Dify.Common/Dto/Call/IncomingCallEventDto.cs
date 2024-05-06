namespace Dify.Common.Dto.Call;

public record IncomingCallEventDto (
    Guid Id,
    string Name,
    CallerInfo Caller,
    IEnumerable<CallerInfo> OtherParticipants
);
