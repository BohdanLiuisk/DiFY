using System;
using MediatR;

namespace DiFY.Modules.UserAccess.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
        Guid Id { get; } 
    }
}