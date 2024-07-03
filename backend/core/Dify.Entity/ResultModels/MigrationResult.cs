namespace Dify.Entity.ResultModels;

public class MigrationResult(bool isSuccess, Exception? exception = null)
{
    public bool IsSuccess { get; internal init; } = isSuccess;
    
    public Exception? Exception { get; internal set; } = exception;
}
