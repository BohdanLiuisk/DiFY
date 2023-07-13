namespace Dify.Common.Models;

public class CommandResponse<T>: BaseRequestResponse
{
    public T Body { get; set; }
    
    public CommandResponse() { }
    
    public CommandResponse(T body) {
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
