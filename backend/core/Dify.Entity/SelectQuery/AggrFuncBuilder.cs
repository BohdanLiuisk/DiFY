using Dify.Entity.SelectQuery.Enums;
using Dify.Entity.SelectQuery.Models;
using SqlKata;

namespace Dify.Entity.SelectQuery;

public class AggrFuncBuilder(Query query, JoinBuilder joinBuilder)
{
    public void AppendAggrFunctions(IEnumerable<SelectExpression> selectExpressions) {
        foreach (var aggrFunction in selectExpressions.Where(e => e.Type == ExpressionType.Function)) {
            AppendAggrFunction(aggrFunction);
        }
    }

    public void AppendAggrFunction(SelectExpression aggrFunction) {
        var pathInfo = joinBuilder.BuildColumnPathInfo(aggrFunction.Path);
        if (string.IsNullOrEmpty(aggrFunction.AggrFunc)) {
            throw new ArgumentException("aggrFunc is required for aggr function");
        }
        if (string.IsNullOrEmpty(aggrFunction.Alias)) {
            throw new ArgumentException("alias is required for aggr function");
        }
        var path = string.Join('.', pathInfo.Path.Split('.').Select(part => $"\"{part}\""));
        query.SelectRaw($"{aggrFunction.AggrFunc}({path}) as {aggrFunction.Alias}");
        aggrFunction.SelectAlias = aggrFunction.Alias;
    }
}
