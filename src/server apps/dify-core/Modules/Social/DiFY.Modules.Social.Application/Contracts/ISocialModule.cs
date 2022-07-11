using System.Threading.Tasks;

namespace DiFY.Modules.Social.Application.Contracts
{
    public interface ISocialModule
    {
        Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

        Task ExecuteCommandAsync(ICommand command);

        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
    }
}
