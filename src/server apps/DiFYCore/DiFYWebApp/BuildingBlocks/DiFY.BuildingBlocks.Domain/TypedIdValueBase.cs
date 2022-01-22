using System;

namespace DiFY.BuildingBlocks.Domain
{
    public class TypedIdValueBase : IEquatable<TypedIdValueBase>
    {
        public  Guid Value { get; }

        protected TypedIdValueBase(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new InvalidOperationException("Id value cannot be empty.");
            }

            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is TypedIdValueBase other && Equals(other);
        }

        public bool Equals(TypedIdValueBase other)
        {
            return Value == other?.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(TypedIdValueBase obj1, TypedIdValueBase obj2)
        {
            return obj1?.Equals(obj2) ?? Equals(obj2, null);
        }

        public static bool operator !=(TypedIdValueBase obj1, TypedIdValueBase obj2)
        {
            return !(obj1 == obj2);
        }
    }
}