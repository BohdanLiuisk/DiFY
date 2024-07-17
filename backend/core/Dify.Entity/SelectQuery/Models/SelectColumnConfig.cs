using System.Text.Json.Serialization;

namespace Dify.Entity.SelectQuery.Models;

public class SelectColumnConfig
{
    public SelectColumnConfig(string path) {
        Path = path;
    }
    
    public SelectColumnConfig(string path, string tableAlias) {
        Path = path;
        Alias = $"{tableAlias}_{path}";
    }
    
    [JsonConstructor]
    public SelectColumnConfig(string path, List<SelectColumnConfig>? columns) {
        Path = path;
        if (columns != null) {
            Columns = columns;
        }
    }
    
    [JsonPropertyName("path")]
    [JsonRequired]
    public string Path { get; set; }
    
    [JsonPropertyName("columns")]
    public List<SelectColumnConfig> Columns { get; set; } = new();
    
    internal string? Alias { get; set; }
}
