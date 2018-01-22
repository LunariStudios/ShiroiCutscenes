namespace Shiroi.Cutscenes.Serialization {
    public class ByteSerializer : Serializer<byte> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetByte(key);
        }

        public override void Serialize(byte value, string name, ISerializedObject destination) {
            destination.SetByte(name, value);
        }
    }

    public class SignedByteSerializer : Serializer<sbyte> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetSignedByte(key);
        }

        public override void Serialize(sbyte value, string name, ISerializedObject destination) {
            destination.SetSignedByte(name, value);
        }
    }

    public class CharSerializer : Serializer<char> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetChar(key);
        }

        public override void Serialize(char value, string name, ISerializedObject destination) {
            destination.SetChar(name, value);
        }
    }

    public class ShortSerializer : Serializer<short> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetShort(key);
        }

        public override void Serialize(short value, string name, ISerializedObject destination) {
            destination.SetShort(name, value);
        }
    }

    public class UnsignedShortSerializer : Serializer<ushort> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetUnsignedShort(key);
        }

        public override void Serialize(ushort value, string name, ISerializedObject destination) {
            destination.SetUnsignedShort(name, value);
        }
    }

    public class IntSerializer : Serializer<int> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetInt(key);
        }

        public override void Serialize(int value, string name, ISerializedObject destination) {
            destination.SetInt(name, value);
        }
    }

    public class UnsignedIntSerializer : Serializer<uint> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetUnsignedInt(key);
        }

        public override void Serialize(uint value, string name, ISerializedObject destination) {
            destination.SetUnsignedInt(name, value);
        }
    }

    public class LongSerializer : Serializer<long> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetLong(key);
        }

        public override void Serialize(long value, string name, ISerializedObject destination) {
            destination.SetLong(name, value);
        }
    }

    public class UnsignedLongSerializer : Serializer<ulong> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetUnsignedLong(key);
        }

        public override void Serialize(ulong value, string name, ISerializedObject destination) {
            destination.SetUnsignedLong(name, value);
        }
    }

    public class FloatSerializer : Serializer<float> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetFloat(key);
        }

        public override void Serialize(float value, string name, ISerializedObject destination) {
            destination.SetFloat(name, value);
        }
    }

    public class DoubleSerializer : Serializer<float> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetDouble(key);
        }

        public override void Serialize(float value, string name, ISerializedObject destination) {
            destination.SetDouble(name, value);
        }
    }

    public class StringSerializer : Serializer<string> {
        public override object Deserialize(string key, ISerializedObject obj) {
            return obj.GetString(key);
        }

        public override void Serialize(string value, string name, ISerializedObject destination) {
            destination.SetString(name, value);
        }
    }
}