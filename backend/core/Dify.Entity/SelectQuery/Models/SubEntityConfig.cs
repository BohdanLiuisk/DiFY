using System.Text.RegularExpressions;

namespace Dify.Entity.SelectQuery.Models;

public class SubEntityConfig
{
    public string Name { get; init; }
    
    public string JoinBy { get; init; }
    
    public string JoinTo { get; init; }
    
    public string? Operator { get; init; }
    
    private SubEntityConfig(string name, string joinBy, string joinTo, string? operatorValue = null) {
        Name = name;
        JoinBy = joinBy;
        JoinTo = joinTo;
        Operator = operatorValue;
    }

    private const string SubEntityFilterPattern = @"\[(.*?)(?::(.*?)(?::(.*?))?)?\]\.(.*)";
    private const string SubEntityPathPattern = @"\[(.*?):(.*?):(.*?)\]";

    public static bool IsSubEntityFilter(string path) {
        return Regex.IsMatch(path, SubEntityFilterPattern);
    }

    public static bool IsSubEntityPath(string path) {
        return Regex.IsMatch(path, SubEntityPathPattern);
    }

    public static SubEntityConfig FromFilterPath(string path) {
        var match = Regex.Match(path, SubEntityFilterPattern);
        if (!match.Success) throw new ArgumentException("Invalid filter path format", nameof(path));
        return new SubEntityConfig(
            name: match.Groups[1].Value,
            joinBy: string.IsNullOrEmpty(match.Groups[2].Value) ? string.Empty : match.Groups[2].Value,
            joinTo: string.IsNullOrEmpty(match.Groups[3].Value) ? string.Empty : match.Groups[3].Value,
            operatorValue: match.Groups[4].Value
        );
    }

    public static SubEntityConfig FromSubEntityPath(string path) {
        var match = Regex.Match(path, SubEntityPathPattern);
        if (!match.Success) throw new ArgumentException("Invalid sub-entity path format", nameof(path));
        return new SubEntityConfig(
            name: match.Groups[1].Value,
            joinBy: match.Groups[2].Value,
            joinTo: match.Groups[3].Value
        );
    }
}
