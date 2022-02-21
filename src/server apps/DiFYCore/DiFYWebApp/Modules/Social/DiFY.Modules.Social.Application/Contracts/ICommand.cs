using MediatR;
using System;

namespace DiFY.Modules.Social.Application.Contracts
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {
        Guid Id { get; }
    }

    public interface ICommand : IRequest<Unit>
    {
        Guid Id { get; }
    }
}
