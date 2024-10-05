using System.Data;

namespace Dify.Entity.Utils;

public static class DbTypeUtils
{
    public static bool GetTypeCanBeChanged(DbType newType, DbType oldType) {
        return (GetIsNumberType(newType) && GetIsNumberType(oldType)) ||
               (GetIsStringType(newType) && GetIsStringType(oldType));
    }
    
    public static bool GetIsNumberType(DbType dbType) {
        return dbType is DbType.Currency or DbType.Decimal 
            or DbType.Int16 or DbType.Int32 or DbType.Int64 or DbType.Single or DbType.Double;
    }
    
    public static bool GetIsStringType(DbType dbType) {
        return dbType is DbType.String;
    }
    
    public static bool GetPrecisionPropertyApplicable(DbType dbType) {
        return dbType is DbType.Decimal;
    }

    public static bool GetSizePropertyApplicable(DbType dbType) {
        return dbType is DbType.String or DbType.Decimal;
    }

    public static bool GetIsDateTimeType(DbType dbType) {
        return dbType is DbType.Date or DbType.DateTime or DbType.DateTimeOffset;
    }
}
