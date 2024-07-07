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
            .Must((column, size) => !DbTypeUtils.GetSizePropertyApplicable((DbType)column.Type) || size.HasValue)
            .WithMessage(c => $"[size] is required for column with type { ((DbType)c.Type).ToString() }");
        RuleFor(c => c.Size)
            .Must((column, size) => {
                if (size.HasValue && (DbType)column.Type == DbType.Decimal) {
                    return size.Value is <= 19 and >= 1;
                }
                return true;
            })
            .WithMessage(c => $"[precision] is not applicable for column with type { ((DbType)c.Type).ToString() }");
        RuleFor(c => c.Precision)
            .Must((column, precision) => {
                if (!precision.HasValue) {
                    return !DbTypeUtils.GetPrecisionPropertyApplicable((DbType)column.Type);
                }
                return true;
            })
            .WithMessage(c => $"[precision] is required for column with type { ((DbType)c.Type).ToString() }");
        RuleFor(c => c.Precision)
            .Must((column, precision) => {
                if (precision.HasValue) {
                    return DbTypeUtils.GetPrecisionPropertyApplicable((DbType)column.Type);
                }
                return true;
            })
            .WithMessage(c => $"[precision] is not applicable for column with type { ((DbType)c.Type).ToString() }");
        RuleFor(c => c.Precision).Must((_, precision) => {
            if (precision.HasValue) {
                return precision.Value is <= 5 and >= 1;
            }
            return true;
        }).WithMessage("[precision] must be <= 5 and >= 1");
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
