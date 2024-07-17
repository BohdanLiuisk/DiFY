using Newtonsoft.Json.Linq;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using SqlKata.Execution;

namespace Dify.Entity.SelectQuery;

public class JsonResultBuilder
{
    private IEnumerable<IDictionary<string, object>>? _resultRows;
    private List<string>? _errors;
    private SelectQueryConfig? _selectConfig;
    private EntityStructure? _rootStructure;
    private PaginationResult<dynamic>? _paginationResult;
    
    public JsonResultBuilder WithSelectQueryConfig(SelectQueryConfig config) { 
        _selectConfig = config;
        return this;
    }

    public JsonResultBuilder WithRootStructure(EntityStructure structure) {
        _rootStructure = structure;
        return this;
    }
    
    public JsonResultBuilder WithErrors(IEnumerable<string> errors) {
        _errors = errors.ToList();
        return this;
    } 
    
    public void WithResultRows(IEnumerable<dynamic> rows) {
        _resultRows = rows.Select<dynamic, IDictionary<string, object>>(row => 
            ((IDictionary<string, object>)row).ToDictionary(
                entry => entry.Key,
                entry => entry.Value));
    }
    
    public void WithPaginationResult(PaginationResult<dynamic> paginationResult) {
        _paginationResult = paginationResult;
        WithResultRows(paginationResult.List);
    }
    
    public JObject BuildSingleRowJson() {
        ValidateParameters();
        var resultObject = GetResultObject();
        var firstRow = _resultRows!.FirstOrDefault();
        if (firstRow != null) {
            var jsonObject = new JObject();
            foreach (var rootColumn in _selectConfig!.Columns) {
                ProcessColumn(firstRow, rootColumn, jsonObject);
            }
            resultObject.AddFirst(new JProperty("data", jsonObject));
        } else {
            resultObject.AddFirst(new JProperty("data", JValue.CreateNull()));
        }
        return resultObject;
    }

    public JObject BuildMultipleRowsJson() {
        ValidateParameters();
        var resultArray = new JArray();
        foreach (var resultRow in _resultRows!) {
            var jsonObject = new JObject();
            foreach (var rootColumn in _selectConfig!.Columns) {
                ProcessColumn(resultRow, rootColumn, jsonObject);
            }
            resultArray.Add(jsonObject);
        }
        var resultObject = GetResultObject(resultArray);
        return resultObject;
    }
    
    public JObject BuildPaginatedRowsJson() {
        ValidateParameters();
        var resultArray = new JArray();
        foreach (var resultRow in _resultRows!) {
            var jsonObject = new JObject();
            foreach (var rootColumn in _selectConfig!.Columns) {
                ProcessColumn(resultRow, rootColumn, jsonObject);
            }
            resultArray.Add(jsonObject);
        }
        var resultJson = GetResultObject(resultArray);
        return resultJson;
    }
    
    private JObject GetResultObject(JArray jArray) {
        var resultObject = GetResultObject();
        resultObject.AddFirst(new JProperty("data", jArray));
        return resultObject;
    }

    private JObject GetResultObject() {
        var resultObject = new JObject {
            { "count", _resultRows?.Count() ?? 0 }
        };
        if (_errors != null && _errors.Count != 0) {
            resultObject.Add("errors", new JArray(_errors));
        }
        if (_paginationResult == null) return resultObject;
        var paginationResult = new JObject {
            { "totalCount", _paginationResult.Count },
            { "page", _paginationResult.Page },
            { "perPage", _paginationResult.PerPage },
            { "totalPages", _paginationResult.TotalPages }
        };
        resultObject.Add("paginationResult", paginationResult);
        return resultObject;
    }

    private void ProcessColumn(IDictionary<string, object> resultRow, SelectColumnConfig rootColumn, 
        JObject jsonObject) {
        var isForeignColumn = _rootStructure?.Columns
            .FirstOrDefault(c => c.Name == rootColumn.Path)?.IsForeignKey ?? false;
        if (!isForeignColumn) {
            AddColumnValue(rootColumn, resultRow, jsonObject);
            return;
        }
        var entityColumn = _rootStructure?.Columns.FirstOrDefault(c => c.Name == rootColumn.Path);
        if (entityColumn?.ForeignKeyStructure?.ReferenceEntityStructure == null) return;
        var entityStructure = entityColumn.ForeignKeyStructure.ReferenceEntityStructure;
        var innerValues = ReadInnerColumns(rootColumn.Columns, resultRow, entityStructure);
        jsonObject.Add(rootColumn.Path, innerValues == null ? JValue.CreateNull() : innerValues);
    }

    private JObject? ReadInnerColumns(List<SelectColumnConfig> columns, IDictionary<string, object> resultRow, 
        EntityStructure parentStructure) {
        var jsonObject = new JObject();
        if (!TryReadPrimaryColumn(columns, resultRow, parentStructure, jsonObject)) {
            return null;
        }
        foreach (var column in columns.Where(c => c.Path != parentStructure.PrimaryColumn.Name)) {
            if (column.Columns.Count == 0) {
                AddColumnValue(column, resultRow, jsonObject);
            } else {
                AddInnerColumns(column, resultRow, parentStructure, jsonObject);
            }
        }
        return jsonObject;
    }

    private bool TryReadPrimaryColumn(List<SelectColumnConfig> columns, IDictionary<string, object> resultRow, 
        EntityStructure parentStructure, JObject jsonObject) {
        var primaryColumnConfig = columns.FirstOrDefault(c => c.Path == parentStructure.PrimaryColumn.Name);
        if (primaryColumnConfig == null || string.IsNullOrEmpty(primaryColumnConfig.Alias)) {
            return false;
        }
        var columnAlias = primaryColumnConfig.Alias;
        if (!resultRow.TryGetValue(columnAlias, out var primaryColumnValue) || primaryColumnValue == null) {
            return false;
        }
        jsonObject.Add(primaryColumnConfig.Path, JToken.FromObject(primaryColumnValue));
        return true;
    }

    private void AddColumnValue(SelectColumnConfig column, IDictionary<string, object> resultRow, JObject jsonObject) {
        if (!string.IsNullOrEmpty(column.Alias) && resultRow.TryGetValue(column.Alias, out var resultValue)) {
            jsonObject.Add(column.Path, JToken.FromObject(resultValue is null ? JValue.CreateNull() : resultValue));
        }
    }

    private void AddInnerColumns(SelectColumnConfig column, IDictionary<string, object> resultRow, 
        EntityStructure parentStructure, JObject jsonObject) {
        var entityColumn = parentStructure.Columns.FirstOrDefault(c => c.Name == column.Path);
        if (entityColumn?.ForeignKeyStructure?.ReferenceEntityStructure == null) return;
        var entityStructure = entityColumn.ForeignKeyStructure.ReferenceEntityStructure;
        var innerValues = ReadInnerColumns(column.Columns, resultRow, entityStructure);
        jsonObject.Add(column.Path, innerValues);
    }

    private void ValidateParameters() {
        if (_resultRows == null || _selectConfig == null || _rootStructure == null) {
            throw new InvalidOperationException($"{nameof(JsonResultBuilder)} is not fully initialized.");
        }
    }
}
