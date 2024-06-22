using System.Data;
using Dify.Entity.Descriptor;
using FluentValidation;

namespace Dify.Entity.Validation;

public class ColumnDescriptorValidator : AbstractValidator<ColumnDescriptor> 
{
    public ColumnDescriptorValidator() {
        RuleFor(c => c.Id)
            .NotEmpty()
            .Must(id => id != Guid.Empty)
            .WithMessage("[Id] must not be empty and should be a valid GUID.");
        RuleFor(c => c.Name).NotEmpty().WithMessage("[Name] must not be empty.");
        RuleFor(c => c.Caption).NotEmpty().WithMessage("[Caption] must not be empty.");
        RuleFor(c => c.Type)
            .Must(columnType => Enum.IsDefined(typeof(DbType), columnType))
            .WithMessage(c => $"Provided [Type] { ((DbType)c.Type).ToString() } is not supported.");
        RuleFor(c => c.Size)
            .Must((column, size) => !GetSizePropertyApplicable((DbType)column.Type) || size.HasValue)
            .WithMessage(c => $"[Size] is required for column with type { ((DbType)c.Type).ToString() }");
        RuleFor(c => c.IsPrimaryKey)
            .Must((column, isPrimaryKey) => {
                if (isPrimaryKey.HasValue && isPrimaryKey.Value) {
                    return (DbType)column.Type == DbType.Int32 || (DbType)column.Type == DbType.Guid;
                }
                return true;
            })
            .WithMessage("[Primary key] should be integer or guid");
    }

    private static bool GetSizePropertyApplicable(DbType dbType) {
        return dbType is DbType.AnsiString or DbType.Binary or DbType.String
            or DbType.VarNumeric or DbType.AnsiStringFixedLength or DbType.StringFixedLength;
    }
}
