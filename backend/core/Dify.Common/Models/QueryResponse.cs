namespace Dify.Common.Models;

public class QueryResponse<T> : BaseRequestResponse
{
    public T Body { get; set; }
    
    public QueryResponse() { }
    
    public QueryResponse(T body)
    {
        Body = body;
        Success = true;
    }

    public override void SetErrorMessage(Exception exception) {
        Error = exception.Message;
        Success = false;
    }

    public void SetBody(T body) {
        Body = body;
        Success = true;
    }
}
