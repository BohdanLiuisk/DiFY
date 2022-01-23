using MediatR;

namespace DiFY.Modules.Administration.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
        
    }
}