using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Calling;

public class CallSummary : ValueObject
{
    public Duration Duration { get; }
    
    public int ParticipantsCount { get;}

    public static CallSummary CreateNew(Duration duration, int participantsCount)
    {
        return new CallSummary(duration, participantsCount);
    }

    private CallSummary(Duration duration, int participantsCount)
    {
        Duration = duration;
        ParticipantsCount = participantsCount;
    }
}