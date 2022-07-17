using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Calling;

public class Duration : ValueObject
{
    public double Value { get; }
    
    private Duration() { }

    private Duration(double minutes)
    {
        Value = minutes;
    }
    
    public static Duration Of(double? minutes)
    {
        return new Duration(minutes ?? 0);
    }
}