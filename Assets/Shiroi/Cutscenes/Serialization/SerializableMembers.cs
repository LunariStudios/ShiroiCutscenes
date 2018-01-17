using System;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes.Serialization {
    public class SerializedBoolean : SerializedMember<bool> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.Boolean;
        }

        public SerializedBoolean(string name, bool value) : base(name, value) { }
    }

    public class SerializedByte : SerializedMember<byte> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.Byte;
        }

        public SerializedByte(string name, byte value) : base(name, value) { }
    }

    public class SerializedChar : SerializedMember<char> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.Char;
        }

        public SerializedChar(string name, char value) : base(name, value) { }
    }

    public class SerializedSignedByte : SerializedMember<sbyte> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.SByte;
        }

        public SerializedSignedByte(string name, sbyte value) : base(name, value) { }
    }

    public class SerializedShort : SerializedMember<short> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.Short;
        }

        public SerializedShort(string name, short value) : base(name, value) { }
    }

    public class SerializedUShort : SerializedMember<ushort> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.UShort;
        }

        public SerializedUShort(string name, ushort value) : base(name, value) { }
    }

    public class SerializedInt : SerializedMember<int> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.Int;
        }

        public SerializedInt(string name, int value) : base(name, value) { }
    }

    public class SerializedUInt : SerializedMember<uint> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.UInt;
        }

        public SerializedUInt(string name, uint value) : base(name, value) { }
    }

    public class SerializedLong : SerializedMember<long> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.Long;
        }

        public SerializedLong(string name, long value) : base(name, value) { }
    }

    public class SerializedULong : SerializedMember<ulong> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.ULong;
        }

        public SerializedULong(string name, ulong value) : base(name, value) { }
    }

    public class SerializedFloat : SerializedMember<float> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.Float;
        }

        public SerializedFloat(string name, float value) : base(name, value) { }
    }

    public class SerializedDouble : SerializedMember<double> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.Double;
        }

        public SerializedDouble(string name, double value) : base(name, value) { }
    }

    public class SerializedDecimal : SerializedMember<decimal> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.Decimal;
        }

        public SerializedDecimal(string name, decimal value) : base(name, value) { }
    }

    public class SerializedString : SerializedMember<string> {
        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.String;
        }

        public SerializedString(string name, string value) : base(name, value) { }
    }

    [Serializable]
    public class SerializedComplex : SerializedMember {
        [SerializeField]
        private SerializedMember[] members;

        [SerializeField]
        private string type;

        public override SerializedMemberType GetMemberType() {
            return SerializedMemberType.Complex;
        }

        public SerializedComplex(string name, object val) : base(name) {
            var t = val.GetType().AssemblyQualifiedName;
            var m = SerializationUtil.Serialize(val);
            this.members = m;
            this.type = t;
        }

        public SerializedComplex(string name, SerializedMember[] members, string type) : base(name) {
            this.members = members;
            this.type = type;
        }

        public static SerializedComplex FromToken(IToken token) {
            return new SerializedComplex(null, token);
        }
    }
}