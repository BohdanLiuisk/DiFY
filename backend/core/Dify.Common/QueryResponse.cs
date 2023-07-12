namespace Dify.Common;

public class QueryResponse<T> : BaseQueryResponse
{
    public T Body { get; set; }

    public override void SetErrorMessage(Exception exception) {
        Error = exception.Message;
        Success = false;
    }

    public void SetBody(T body) {
        Body = body;
        Success = true;
    }
}
