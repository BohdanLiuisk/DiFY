﻿using Dify.Entity.Structure;

namespace Dify.Entity.Validation;

public class EntityValidationResult
{
    private readonly List<EntityStructureError> _errors = new();
    
    public required string EntityName { get; init; }
    
    public bool Success { get; private set; } = true;
    
    public IEnumerable<EntityStructureError> Errors => _errors;
    
    public void AddTableError(string tableName, string error) {
        Success = false;
        _errors.Add(new EntityStructureError {
            ElementType = EntityStructureElementType.Table,
            ElementName = tableName,
            Error = error
        });
    }
    
    public void AddColumnError(string columnName, string error) {
        Success = false;
        _errors.Add(new EntityStructureError {
            ElementType = EntityStructureElementType.Column,
            ElementName = columnName,
            Error = error
        });
    }
    
    public void AddErrors(IEnumerable<EntityStructureError> errors) {
        Success = false;
        _errors.AddRange(errors);
    }

    public CreateEntityResult ToCreateEntityResult() {
        var result = new CreateEntityResult {
            EntityName = EntityName
        };
        result.AddErrors(_errors);
        return result;
    }
}

