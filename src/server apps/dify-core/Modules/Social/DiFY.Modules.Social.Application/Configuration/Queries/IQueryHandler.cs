using DiFY.Modules.Social.Application.Contracts;
using MediatR;

namespace DiFY.Modules.Social.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {

    }
}
