using System.Text.Json.Serialization;
using Dify.Entity.SelectQuery.Enums;

namespace Dify.Entity.SelectQuery.Models;

public class SelectExpression
{
    public SelectExpression(string path) {
        Path = path;
    }
    
    private SelectExpression(string path, string? alias) {
        Type = ExpressionType.Column;
        Path = path;
        Alias = alias;
    }
    
    [JsonConstructor]
    public SelectExpression(string path, List<SelectExpression>? columns) {
        Path = path;
        if (columns != null) {
            Columns = columns;
            SetParentColumn(this, columns);
        }
    }
    
    [JsonPropertyName("type")]
    public ExpressionType Type { get; set; } = ExpressionType.Column;
    
    [JsonPropertyName("path")]
    public string Path { get; set; }
    
    [JsonPropertyName("alias")]
    public string? Alias { get; set; }
    
    [JsonPropertyName("columns")]
    public List<SelectExpression> Columns { get; set; } = new();
    
    [JsonPropertyName("subEntity")]
    public string? SubEntity { get; set; }
    
    [JsonPropertyName("aggrFunc")]
    public string? AggrFunc { get; set; }
    
    [JsonPropertyName("filter")]
    public SelectFilter? Filter { get; set; }
    
    [JsonPropertyName("subQuerySorting")]
    public List<SortConfig> SubQuerySorting { get; set; } = new();
    
    internal string? SelectAlias { get; set; }
    
    internal SelectExpression? ParentColumn { get; set; }
    
    public static SelectExpression Column(string path, string? alias = null) {
        return new SelectExpression(path, alias);
    }
    
    private void SetParentColumn(SelectExpression parent, List<SelectExpression> columns) {
        foreach (var column in columns) {
            column.ParentColumn = parent;
            SetParentColumn(column, column.Columns);
        }
    }
    
    public string GetFullPath() {
        if (ParentColumn == null) {
            return Path;
        }
        return ParentColumn.GetFullPath() + "." + Path;
    }
}
