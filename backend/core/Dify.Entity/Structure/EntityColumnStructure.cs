using System.Data;
using System.Text.Json.Serialization;
using Dify.Entity.Descriptor;
using Dify.Entity.ResultModels;
using Dify.Entity.Utils;

namespace Dify.Entity.Structure;

public class EntityColumnStructure
{
    internal EntityColumnStructure(EntityStructure entityStructure, ColumnDescriptor columnDescriptor) {
        var columnType = (DbType)columnDescriptor.Type;
        var dbName = GetColumnDbName(columnDescriptor);
        var isUnique = columnDescriptor.IsUnique ?? false;
        var isPrimaryKey = columnDescriptor.IsPrimaryKey ?? false;
        Id = columnDescriptor.Id;
        EntityId = entityStructure.Id;
        EntityStructure = entityStructure;
        Name = columnDescriptor.Name;
        Caption = columnDescriptor.Caption;
        Type = columnType;
        TableName = entityStructure.Name;
        DbName = dbName;
        IsUnique = isUnique;
        IsPrimaryKey = isPrimaryKey;
        SetSize(columnDescriptor.Size);
        SetPrecision(columnDescriptor.Precision);
        SetIsNullable(columnDescriptor.IsNullable);
        State = EntityStructureElementState.New;
    }
    
    [JsonConstructor]
    internal EntityColumnStructure(Guid id, string caption, string name, string dbName, Guid entityId, string tableName,
        DbType type, bool isNullable, bool isPrimaryKey, bool isUnique, int? size, int? precision, bool isForeignKey, 
        Guid? foreignKeyStructureId) {
        Id = id;
        Caption = caption;
        Name = name;
        DbName = dbName;
        EntityId = entityId;
        TableName = tableName;
        Type = type;
        IsNullable = isNullable;
        IsPrimaryKey = isPrimaryKey;
        IsUnique = isUnique;
        Size = size;
        Precision = precision;
        IsForeignKey = isForeignKey;
        ForeignKeyStructureId = foreignKeyStructureId;
    }
    
    [JsonPropertyName("id")]
    public Guid Id { get; }
    
    [JsonPropertyName("caption")]
    public string Caption { get; private set; }
    
    [JsonPropertyName("name")]
    public string Name { get; private set; }
    
    [JsonPropertyName("dbName")]
    public string DbName { get; private set; }
    
    [JsonPropertyName("entityId")]
    public Guid EntityId { get; }

    [JsonPropertyName("tableName")]
    public string TableName { get; }

    [JsonPropertyName("type")]
    public DbType Type { get; private set; }
    
    [JsonPropertyName("isNullable")]
    public bool IsNullable { get; private set; }
    
    [JsonPropertyName("isPrimaryKey")]
    public bool IsPrimaryKey { get; }
    
    [JsonPropertyName("isUnique")]
    public bool IsUnique { get; }
    
    [JsonPropertyName("size")]
    public int? Size { get; private set; }
    
    [JsonPropertyName("precision")]
    public int? Precision { get; private set; }
    
    [JsonPropertyName("isForeignKey")]
    public bool IsForeignKey { get; private set; }
    
    [JsonPropertyName("foreignKeyStructureId")]
    public Guid? ForeignKeyStructureId { get; private set; }

    [JsonIgnore] 
    public EntityStructure EntityStructure { get; internal set; } = null!;
    
    [JsonIgnore]
    public EntityForeignKeyStructure? ForeignKeyStructure { get; internal set; }
    
    [JsonIgnore]
    public EntityStructureElementState State { get; internal set; }

    internal void DefineForeignKey(EntityForeignKeyStructure foreignKey) {
        IsForeignKey = true;
        ForeignKeyStructureId = foreignKey.Id;
        ForeignKeyStructure = foreignKey;
    }
    
    internal AlterColumnResult AlterColumn(ColumnDescriptor columnDescriptor) {
        var validationResult = ValidateColumnChange(columnDescriptor);
        if (!validationResult.IsSuccess) {
            return validationResult;
        }
        ApplyChanges(columnDescriptor);
        var isChanged = State == EntityStructureElementState.Updated;
        return new AlterColumnResult(this, isChanged);
    }
    
    private void ApplyChanges(ColumnDescriptor columnDescriptor) {
        if (columnDescriptor.Caption != Caption) {
            Caption = columnDescriptor.Caption;
            State = EntityStructureElementState.Updated;
        }
        if (columnDescriptor.Type != (int)Type) {
            Type = (DbType)columnDescriptor.Type;
            State = EntityStructureElementState.Updated;
        }
        if (!IsNullable && columnDescriptor.IsNullable == true && !IsPrimaryKey) {
            IsNullable = true;
            State = EntityStructureElementState.Updated;
        }
        if (columnDescriptor.Precision != Precision) {
            SetPrecision(columnDescriptor.Precision);
            State = EntityStructureElementState.Updated;
        }
        UpdateSize(columnDescriptor.Size);
    }
    
    private AlterColumnResult ValidateColumnChange(ColumnDescriptor columnDescriptor) {
        var alterResult = new AlterColumnResult(this);
        var newType = (DbType)columnDescriptor.Type;
        if (columnDescriptor.Name != Name) {
            alterResult.AddError("Column name cannot be changed");
        }
        if (columnDescriptor.IsNullable == false && IsNullable) {
            alterResult.AddError("Nullable column cannot be not nullable (for now)");
        }
        if (IsPrimaryKey && columnDescriptor.IsNullable.HasValue && columnDescriptor.IsNullable.Value) {
            alterResult.AddError("Primary key cannot be nullable");
        }
        if (IsForeignKey && ForeignKeyStructure != null && columnDescriptor.ReferenceEntityId.HasValue &&
            columnDescriptor.ReferenceEntityId.Value != ForeignKeyStructure.ReferenceEntityId) {
            alterResult.AddError("Foreign entity id cannot be changed");
        }
        if (columnDescriptor.Precision != null && columnDescriptor.Precision != Precision && 
            !DbTypeUtils.GetPrecisionPropertyApplicable(newType)) {
            alterResult.AddError("Precision cannot be changed or not applicable for this type");
        }
        if (columnDescriptor.Size != null && columnDescriptor.Size != Size && 
            !DbTypeUtils.GetSizePropertyApplicable(newType)) {
            alterResult.AddError("Size cannot be changed or not applicable for this type");
        }
        if (columnDescriptor.Size != null && columnDescriptor.Size < Size) {
            alterResult.AddError("Size cannot be changed to smaller value");
        }
        if (columnDescriptor.Size != null && Size == null && Type == DbType.String) {
            alterResult.AddError("Size cannot be changed on unlimited text type");
        }
        if (newType != Type && !DbTypeUtils.GetTypeCanBeChanged(newType, Type)) {
            alterResult.AddError($"Current type {Type.ToString()} cannot be changed to {newType.ToString()}");
        }
        return alterResult;
    }

    private void UpdateSize(int? size) {
        if (size != Size) {
            SetSize(size);
            State = EntityStructureElementState.Updated;
            return;
        }
        if (Type == DbType.Decimal && size == null && Size != Constants.DecimalDefaultSize) {
            SetSize(size);
            State = EntityStructureElementState.Updated;
        }
    }

    private void SetSize(int? size) {
        if (!DbTypeUtils.GetSizePropertyApplicable(Type)) {
            Size = null;
            return;
        }
        Size = Type switch {
            DbType.Decimal => size ?? Constants.DecimalDefaultSize,
            _ => size
        };
    }

    private void SetPrecision(int? precision) {
        if (!DbTypeUtils.GetPrecisionPropertyApplicable(Type)) {
            Precision = null;
            return;
        }
        Precision = precision ?? Constants.DecimalDefaultPrecision;
    }

    private void SetIsNullable(bool? isNullable) {
        IsNullable = isNullable ?? true;
    }
    
    private static string GetColumnDbName(ColumnDescriptor columnDescriptor) {
        if (columnDescriptor.ReferenceEntityId != null) {
            return $"{columnDescriptor.Name}_id";
        }
        return columnDescriptor.Name;
    }
}
