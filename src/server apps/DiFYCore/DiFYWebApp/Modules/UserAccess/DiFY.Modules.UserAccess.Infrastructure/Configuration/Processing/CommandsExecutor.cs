using System.Threading.Tasks;
using Autofac;
using DiFY.Modules.UserAccess.Application.Contracts;
using MediatR;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    internal static class CommandsExecutor
    {
        internal static async Task Execute(ICommand command)
        {
            await using var scope = UserAccessCompositionRoot.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(command);
        }

        internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            await using var scope = UserAccessCompositionRoot.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}