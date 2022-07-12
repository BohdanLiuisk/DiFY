using System.Threading.Tasks;
using Autofac;
using DiFY.Modules.Administration.Application.Contracts;
using DiFY.Modules.Administration.Infrastructure.Configuration;
using DiFY.Modules.Administration.Infrastructure.Configuration.Processing;
using MediatR;

namespace DiFY.Modules.Administration.Infrastructure
{
    public class AdministrationModule : IAdministrationModule
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
            using var scope = AdministrationCompositionRoot.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(query);
        }
    }
}