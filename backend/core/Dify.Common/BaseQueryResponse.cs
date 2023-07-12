namespace Dify.Common;

public abstract class BaseQueryResponse
{
    public bool Success { get; set; }
    public string Error { get; set; }

    public abstract void SetErrorMessage(Exception exception);
}
