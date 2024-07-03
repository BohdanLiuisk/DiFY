using System.Text.Json;

namespace Dify.Entity.Exceptions;

public class EntityStructureException : Exception
{
    public string? Details { get; protected set; }
    
    protected EntityStructureException(string message, string? details = null) 
        : base(FormatMessage(message, details)) {
        Details = details;
    }
    
    protected static string? SerializeResult<T>(T? result) {
        return result == null ? null : JsonSerializer.Serialize(result);
    }

    protected static string? SerializeResults<T>(IList<T>? results) {
        if (results == null || results.Count == 0) {
            return null;
        }
        return JsonSerializer.Serialize(results);
    }

    private static string FormatMessage(string message, string? details) {
        return string.IsNullOrEmpty(details) ? message : $"{message}\n{details}";
    }
}
