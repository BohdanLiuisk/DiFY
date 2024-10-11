using Newtonsoft.Json.Linq;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Utils;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Dify.Entity.SelectQuery;

public class SelectResultBuilder(SelectQueryConfig selectConfig)
{
    private Query? _query;
    private IEnumerable<dynamic>? _rows;
    private PaginationResult<dynamic>? _paginationResult;
    private readonly List<string> _errors = new();

    public SelectResultBuilder WithQuery(Query query) {
        _query = query;
        return this;
    }

    public SelectResultBuilder WithRows(IEnumerable<dynamic> rows) {
        _rows = rows;
        return this;
    }

    public SelectResultBuilder WithPagination(PaginationResult<dynamic> paginationResult) {
        _paginationResult = paginationResult;
        return this;
    }

    public SelectResultBuilder WithError(string error) {
        _errors.Add(error);
        return this;
    }

    public JObject Build() {
        if (_errors.Count > 0) {
            return CreateErrorResult();
        }
        var resultObject = new JObject {
            { "data", CreateDataResult(_rows) },
            { "count", _rows?.Count() ?? 0 },
            { "errors", new JArray() }
        };
        if (_paginationResult != null) {
            AddPaginationResult(resultObject, _paginationResult);
        }
        if (selectConfig.Debug == true) {
            resultObject.Add("sql", JToken.FromObject(GetSqlText()));
        }
        return resultObject;
    }

    private JToken CreateDataResult(IEnumerable<dynamic>? rows) {
        if (rows == null) return new JArray();
        var resultRows = SelectQueryUtils.GetResultRows(rows).ToList();
        if (selectConfig.Limit == 1) {
            return CreateRowJson(resultRows.FirstOrDefault());
        }
        return CreateRowsJson(resultRows);
    }

    private JObject CreateErrorResult() {
        var resultObject = new JObject {
            { "data", JValue.CreateNull() },
            { "count", 0 },
            { "errors", new JArray(_errors) }
        };
        if (selectConfig.Debug == true) {
            resultObject.Add("sql", JToken.FromObject(GetSqlText()));
        }
        return resultObject;
    }

    private static void AddPaginationResult(JObject resultObject, PaginationResult<dynamic> paginationResult) {
        var paginationResultObject = new JObject {
            { "totalCount", paginationResult.Count },
            { "page", paginationResult.Page },
            { "perPage", paginationResult.PerPage },
            { "totalPages", paginationResult.TotalPages }
        };
        resultObject.Add("paginationResult", paginationResultObject);
    }

    private JToken CreateRowJson(IDictionary<string, object>? row) {
        if(row == null) return JValue.CreateNull();
        if (selectConfig.RootStructure == null) {
            throw new InvalidOperationException("Root structure is null");
        }
        var reader = new RowReader(row, selectConfig.RootStructure);
        return reader.ReadExpressions(selectConfig.Expressions);
    }

    private JArray CreateRowsJson(IEnumerable<IDictionary<string, object>> rows) {
        var resultArray = new JArray();
        foreach (var row in rows) {
            resultArray.Add(CreateRowJson(row));
        }
        return resultArray;
    }

    private string GetSqlText() {
        var compiler = new PostgresCompiler();
        var sqlResult = compiler.Compile(_query);
        return sqlResult.Sql;
    }
}
