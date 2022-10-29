namespace DiFY.BuildingBlocks.Application.Queries;

public class PagedQueryManager
{
    public const string Offset = "Offset";

    public const string Next = "Next";
    
    public static PageData GetPageData(IPagedQuery query)
    {
        int offset;
        if (!query.Page.HasValue || !query.PerPage.HasValue)
        {
            offset = 0;
        }
        else
        {
            offset = (query.Page.Value - 1) * query.PerPage.Value;
        }
        var next = query.PerPage ?? int.MaxValue;
        return new PageData(offset, next);
    }

    public static string AppendPageStatement(string sql) => 
        $"{sql} OFFSET @{Offset} ROWS FETCH NEXT @{Next} ROWS ONLY;";
}