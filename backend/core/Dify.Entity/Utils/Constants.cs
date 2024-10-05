namespace Dify.Entity.Utils;

public static class Constants
{
    public const int DecimalMaxCapacity = 1000;
    public const int DecimalDefaultSize = 19;
    public const int DecimalDefaultPrecision = 5;
    
    public static class Select
    {
        public const string Clause = "clause";
        public const string Group = "group";
        public const string Or = "or";
        public const string And = "and";
        public const string Exists = "exists";
        public const string NotExists = "notExists";
        public const string Count = "count";
        public const string Min = "min";
        public const string Max = "max";
        public const string Avg = "avg";
        public const string Sum = "sum";
        public const string Asc = "asc";
        public const string Desc = "desc";
        public const string IsNull = "isNull";
        public const string IsNotNull = "isNotNull";
        public const string In = "in";
        public const string Contains = "cts";
        public const string NotContains = "ncts";
        public const string StartsWith = "stw";
        public const string NotStartsWith = "nstw";
        public const string EndsWith = "edw";
        public const string NotEndsWith = "nedw";
    }
}
