using System.Data;
using Dify.Entity.Descriptor;
using Dify.Entity.Utils;
using FluentValidation;

namespace Dify.Entity.Validation;

public class ColumnDescriptorValidator : AbstractValidator<ColumnDescriptor> 
{
    public ColumnDescriptorValidator() {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("[id] must not be empty.")
            .Must(id => id != Guid.Empty).WithMessage("[id] must be a valid GUID.");
        RuleFor(c => c.Name).NotEmpty().WithMessage("[name] must not be empty.");
        RuleFor(c => c.Caption).NotEmpty().WithMessage("[caption] must not be empty.");
        RuleFor(c => c.Type)
            .Must(columnType => Enum.IsDefined(typeof(DbType), columnType))
            .WithMessage(c => $"Provided [type] { ((DbType)c.Type).ToString() } is not supported.");
        RuleFor(c => c.IsNullable)
            .Must((column, isNullable) => 
                !((column.IsPrimaryKey ?? false) && (isNullable ?? false)))
            .WithMessage("Primary key cannot be nullable.");
        RuleFor(c => c.Size)
            .Must((column, size) => {
                if (size.HasValue && !DbTypeUtils.GetSizePropertyApplicable((DbType)column.Type)) {
                    return false;
                }
                return true;
            })
            .WithMessage(c => $"[size] is not applicable for type { ((DbType)c.Type).ToString() }");
        RuleFor(c => c.Size)
            .Must((column, size) => {
                if (size.HasValue && (DbType)column.Type == DbType.Decimal) {
                    return size.Value is <= Constants.DecimalMaxCapacity and >= 1;
                }
                return true;
            })
            .WithMessage("[size] must be in range 1 - 1000 for Decimal type");
        RuleFor(c => c.Precision)
            .Must((column, precision) => (DbType)column.Type == DbType.Decimal || !precision.HasValue)
            .WithMessage(c => $"[precision] is not applicable for type { ((DbType)c.Type).ToString() }");
        RuleFor(c => c.Precision).Must((column, precision) => {
            if (precision.HasValue && (DbType)column.Type == DbType.Decimal) {
                var size = column.Size ?? Constants.DecimalDefaultSize;
                return precision.Value <= size;
            }
            return true;
        }).WithMessage("[precision] must be less than size");
        RuleFor(c => c.IsPrimaryKey)
            .Must((column, isPrimaryKey) => {
                if (isPrimaryKey.HasValue && isPrimaryKey.Value) {
                    var dbType = (DbType)column.Type;
                    return dbType is DbType.Int32 or DbType.Guid;
                }
                return true;
            })
            .WithMessage("[isPrimaryKey] must be integer or guid");
    }
}
