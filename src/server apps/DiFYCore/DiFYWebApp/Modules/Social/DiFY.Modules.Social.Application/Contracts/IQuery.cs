using MediatR;

namespace DiFY.Modules.Social.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {

    }
}
