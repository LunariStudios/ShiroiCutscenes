using Shiroi.Cutscenes.Tokens;

namespace Shiroi.Cutscenes.Serialization {
    public abstract class SerializedMember {
        public string Name;

        protected SerializedMember(string name) {
            Name = name;
        }

        public abstract SerializedMemberType GetMemberType();
    }

    public abstract class SerializedMember<T> : SerializedMember {
        protected SerializedMember(string name, T value) : base(name) {
            Value = value;
        }

        public T Value;
    }

    public enum SerializedMemberType {
        Boolean,
        Byte,
        Char,
        SByte,
        Short,
        UShort,
        Int,
        UInt,
        Long,
        ULong,
        Float,
        Double,
        Decimal,
        String,
        Complex
    }
}