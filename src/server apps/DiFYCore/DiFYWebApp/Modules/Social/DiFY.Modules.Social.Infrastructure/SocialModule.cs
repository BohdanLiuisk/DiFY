using Autofac;
using DiFY.Modules.Social.Application.Contracts;
using DiFY.Modules.Social.Infrastructure.Configuration;
using DiFY.Modules.Social.Infrastructure.Configuration.Processing;
using MediatR;
using System.Threading.Tasks;

namespace DiFY.Modules.Social.Infrastructure
{
    internal class SocialModule : ISocialModule
    {
        public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
        {
            return await CommandsExecutor.Execute(command);
        }

        public async Task ExecuteCommandAsync(ICommand command)
        {
            await CommandsExecutor.Execute(command);
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            using var scope = SocialCompositionRoot.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(query);
        }
    }
}
