using Newtonsoft.Json.Linq;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Dify.Entity.Utils;
using SqlKata.Execution;

namespace Dify.Entity.SelectQuery;

public class SelectResultBuilder(SelectQueryConfig selectConfig, EntityStructure rootStructure)
{
    private List<string>? _errors;
    
    public SelectResultBuilder WithErrors(IEnumerable<string> errors) {
        _errors = errors.ToList();
        return this;
    }
    
    public JObject BuildSingleRowJson(IDictionary<string, object>? firstRow) {
        var data = firstRow == null ? JValue.CreateNull() : CreateRowJson(firstRow);
        return CreateResultObject(data, count: 1);
    }

    public JObject BuildMultipleRowsJson(IEnumerable<dynamic> rawRows) {
        var resultRows = SelectQueryUtils.GetResultRows(rawRows).ToList();
        var resultArray = CreateRowsJson(resultRows);
        return CreateResultObject(resultArray, resultRows.Count);
    }

    public JObject BuildPaginatedRows(PaginationResult<dynamic> paginationResult) {
        var resultRows = SelectQueryUtils.GetResultRows(paginationResult.List).ToList();
        var resultArray = CreateRowsJson(resultRows);
        var resultObject = CreateResultObject(resultArray, resultRows.Count);
        var paginationResultObject = new JObject {
            { "totalCount", paginationResult.Count },
            { "page", paginationResult.Page },
            { "perPage", paginationResult.PerPage },
            { "totalPages", paginationResult.TotalPages }
        };
        resultObject.Add("paginationResult", paginationResultObject);
        return resultObject;
    }

    private JObject CreateResultObject(JToken data, int count) {
        var resultObject = new JObject {
            { "data", data },
            { "count", count }
        };
        if (_errors != null && _errors.Count != 0) {
            resultObject.Add("errors", new JArray(_errors));
        }
        return resultObject;
    }

    private JToken CreateRowJson(IDictionary<string, object> row) {
        var reader = new RowReader(row, rootStructure);
        return reader.ReadExpressions(selectConfig.Expressions);
    }

    private JArray CreateRowsJson(IEnumerable<IDictionary<string, object>> rows) {
        var resultArray = new JArray();
        foreach (var row in rows) {
            resultArray.Add(CreateRowJson(row));
        }
        return resultArray;
    }
}
