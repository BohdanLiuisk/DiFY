using Dify.Entity.SelectQuery.Models;
namespace Dify.Entity.Abstract;

public interface ISelectQueryExecutor
{
    Task<string> ExecuteAsync(SelectQueryConfig selectConfig);

    Task<SelectQueryResult<T>?> ExecuteAsync<T>(SelectQueryConfig selectConfig);
}
