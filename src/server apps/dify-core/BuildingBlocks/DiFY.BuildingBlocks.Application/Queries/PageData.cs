namespace DiFY.BuildingBlocks.Application.Queries;

public struct PageData
{
    public int Offset { get; }

    public int Next { get; }
    
    public PageData(int offset, int next)
    {
        Offset = offset;
        Next = next;
    }
}