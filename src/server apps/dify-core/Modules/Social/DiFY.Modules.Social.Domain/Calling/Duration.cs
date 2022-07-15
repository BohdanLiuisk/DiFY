using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Calling;

public class Duration : ValueObject
{
    public double Value { get; private set; }

    private Duration(double minutes)
    {
        Value = minutes;
    }
    
    public static Duration Of(double minutes)
    {
        return new Duration(minutes);
    }
}