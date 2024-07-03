using Dify.Entity.Descriptor;
using Dify.Entity.Structure;

namespace Dify.Entity.Validation;

public static class EntityStructureValidator
{
    public static EntityValidationResult ValidateTableDescriptor(TableDescriptor tableDescriptor) {
        var validationResult = new EntityValidationResult {
            EntityName = tableDescriptor.Name
        };
        var columnErrors = GetInvalidDescriptorColumns(tableDescriptor).ToList();
        if (columnErrors.Count != 0) {
            validationResult.AddErrors(columnErrors);
        }
        var duplicatedColumns = GetDuplicatedColumnNames(tableDescriptor.Columns).ToList();
        if (duplicatedColumns.Count != 0) {
            var error = $"Found duplicated columns: {string.Join(", ", duplicatedColumns)}";
            validationResult.AddTableError(tableDescriptor.Name, error);
        }
        var primaryKeyColumnsCount = tableDescriptor.Columns.Count(c => c.IsPrimaryKey == true);
        if (primaryKeyColumnsCount != 1) {
            var error = primaryKeyColumnsCount > 1
                ? "Table can contain only 1 primary key"
                : "Table must contain at least 1 primary key";
            validationResult.AddTableError(tableDescriptor.Name, error);
        }
        return validationResult;
    }

    public static EntityValidationResult ValidateEntityStructure(EntityStructure entityStructure, 
        IList<EntityStructure> allStructures) {
        var validationResult = new EntityValidationResult {
            EntityName = entityStructure.Name
        };
        ValidateForeignKeys(entityStructure, allStructures, validationResult);
        ValidateDeletedColumns(entityStructure, validationResult);
        return validationResult;
    }

    private static void ValidateDeletedColumns(EntityStructure entityStructure, 
        EntityValidationResult validationResult) {
        var deletedColumns = entityStructure.Columns
            .Where(c => c.State == EntityStructureElementState.Deleted)
            .ToList();
        var deletedPrimaryKey = deletedColumns.FirstOrDefault(c => c.IsPrimaryKey);
        if (deletedPrimaryKey != null) {
            validationResult.AddColumnError(deletedPrimaryKey.Name, "Primary key column can't be deleted");
        }
    }

    private static void ValidateForeignKeys(EntityStructure entityStructure, IList<EntityStructure> allStructures, 
        EntityValidationResult validationResult) {
        var foreignKeyColumns = entityStructure.Columns.Where(
            c => c is { IsForeignKey: true, ForeignKeyStructure: not null });
        foreach (var foreignKeyColumn in foreignKeyColumns) {
            var foreignKeyStructure = foreignKeyColumn.ForeignKeyStructure;
            var referenceEntity = allStructures.FirstOrDefault(
                s => s.Id == foreignKeyStructure!.ReferenceEntityId);
            if (referenceEntity == null) {
                var error = $"Reference entity {foreignKeyStructure!.ReferenceEntityName} not found.";
                validationResult.AddColumnError(foreignKeyColumn.Name, error);
            } else {
                var refEntityPrimaryColumn = referenceEntity.Columns.FirstOrDefault(c => c.IsPrimaryKey);
                if (foreignKeyColumn.Type != refEntityPrimaryColumn!.Type) {
                    var error = $"Column type does not match reference entity primary column type " +
                                $"{refEntityPrimaryColumn.Type.ToString()}";
                    validationResult.AddColumnError(foreignKeyColumn.Name, error);
                }
            }
        }
    }

    private static IEnumerable<EntityStructureError> GetInvalidDescriptorColumns(TableDescriptor tableDescriptor) {
        var columnValidator = new ColumnDescriptorValidator();
        var columnErrors = new List<EntityStructureError>();
        foreach (var columnDescriptor in tableDescriptor.Columns) {
            var result = columnValidator.Validate(columnDescriptor);
            if (result.IsValid) continue;
            columnErrors.AddRange(result.Errors.Select(e => new EntityStructureError {
                ElementType = EntityStructureElementType.Column,
                ElementName = columnDescriptor.Name,
                Error = e.ErrorMessage
            }));
        }
        return columnErrors;
    }

    private static IEnumerable<string> GetDuplicatedColumnNames(IEnumerable<ColumnDescriptor> columns) {
        return columns.GroupBy(column => column.Name)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key);
    }
}
