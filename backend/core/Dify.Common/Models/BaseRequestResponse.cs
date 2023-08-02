namespace Dify.Common.Models;

public abstract class BaseRequestResponse
{
    public bool Success { get; set; }
    public string Error { get; set; }

    public abstract void SetErrorMessage(Exception exception);
}
