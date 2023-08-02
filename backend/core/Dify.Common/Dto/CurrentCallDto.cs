namespace Dify.Common.Dto;

public record CurrentCallDto(
    CallDto Call,
    ICollection<CallParticipantDto> Participants
);
