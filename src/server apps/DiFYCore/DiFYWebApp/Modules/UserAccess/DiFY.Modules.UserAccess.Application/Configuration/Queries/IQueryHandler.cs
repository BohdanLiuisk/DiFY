using DiFY.Modules.UserAccess.Application.Contracts;
using MediatR;

namespace DiFY.Modules.UserAccess.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        
    }
}