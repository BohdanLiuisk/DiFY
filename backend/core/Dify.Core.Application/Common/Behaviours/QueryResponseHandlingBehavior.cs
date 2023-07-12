using Dify.Common.Models;
using MediatR;

namespace Dify.Core.Application.Common.Behaviours;

public class QueryResponseHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TResponse : BaseQueryResponse, new()
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var response = new TResponse();
            response.SetErrorMessage(ex);
            return response;
        }
    }
}
