namespace Dify.Common.Dto.Call;

public record IncomingCallEventDto (
    Guid CallId,
    string CallName,
    int CallerId,
    string CallerName
);
