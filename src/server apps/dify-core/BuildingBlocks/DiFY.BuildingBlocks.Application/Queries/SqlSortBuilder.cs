using System.Linq;
using System.Text;

namespace DiFY.BuildingBlocks.Application.Queries;

public static class SqlSortBuilder
{
    public static string BuildSorting(SortOption[] sortOptions, string? tableAlias)
    {
        if (sortOptions is null || !sortOptions.Any()) return string.Empty;
        var sb = new StringBuilder();
        var filteredSortOptions = sortOptions.DistinctBy(s => s.Column).OrderBy(s => s.SeqNumber).ToArray();
        foreach (var sortOption in filteredSortOptions)
        {
            if (string.IsNullOrWhiteSpace(sortOption.Column)) continue;
            if (!string.IsNullOrWhiteSpace(tableAlias))
            {
                sb.Append(tableAlias).Append($".[{sortOption.Column}]");
            }
            else
            {
                sb.Append($"[{sortOption.Column}]");
            }

            sb.Append(GetSortDirection(sortOption.Direction));
            if (sortOption.Column != filteredSortOptions.LastOrDefault()?.Column)
            {
                sb.Append(',');
            }
        }
        return sb.ToString();
    }

    private static string GetSortDirection(string direction)
    {
        if (string.IsNullOrWhiteSpace(direction)) return string.Empty;
        var lower = direction.ToLower();
        return lower is "asc" or "desc" ? lower.ToUpper() : string.Empty;
    }
}