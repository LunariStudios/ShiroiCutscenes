namespace Shiroi.Cutscenes.Serialization {
    public class ByteSerializer : Serializer<byte> {
        public override object Deserialize(string key, SerializedObject obj) {
            return (byte) obj.GetInt(key);
        }

        public override void Serialize(byte value, string name, SerializedObject destination) {
            destination.SetInt(name, value);
        }
    }

    public class SignedByteSerializer : Serializer<sbyte> {
        public override object Deserialize(string key, SerializedObject obj) {
            return (sbyte) obj.GetInt(key);
        }

        public override void Serialize(sbyte value, string name, SerializedObject destination) {
            destination.SetInt(name, value);
        }
    }

    public class ShortSerializer : Serializer<short> {
        public override object Deserialize(string key, SerializedObject obj) {
            return (short) obj.GetInt(key);
        }

        public override void Serialize(short value, string name, SerializedObject destination) {
            destination.SetInt(name, value);
        }
    }

    public class UnsignedShortSerializer : Serializer<ushort> {
        public override object Deserialize(string key, SerializedObject obj) {
            return (ushort) obj.GetInt(key);
        }

        public override void Serialize(ushort value, string name, SerializedObject destination) {
            destination.SetInt(name, value);
        }
    }


    public class UnsignedIntSerializer : Serializer<uint> {
        public override object Deserialize(string key, SerializedObject obj) {
            return (uint) obj.GetInt(key);
        }

        public override void Serialize(uint value, string name, SerializedObject destination) {
            destination.SetInt(name, (int) value);
        }
    }

    public class LongSerializer : Serializer<long> {
        public override object Deserialize(string key, SerializedObject obj) {
            return (long) obj.GetInt(key);
        }

        public override void Serialize(long value, string name, SerializedObject destination) {
            destination.SetInt(name, (int) value);
        }
    }

    public class UnsignedLongSerializer : Serializer<ulong> {
        public override object Deserialize(string key, SerializedObject obj) {
            return (ulong) obj.GetInt(key);
        }

        public override void Serialize(ulong value, string name, SerializedObject destination) {
            destination.SetInt(name, (int) value);
        }
    }

    public class DoubleSerializer : Serializer<double> {
        public override object Deserialize(string key, SerializedObject obj) {
            return (float) obj.GetInt(key);
        }

        public override void Serialize(double value, string name, SerializedObject destination) {
            destination.SetFloat(name, (float) value);
        }
    }


    public class BooleanSerializer : Serializer<bool> {
        public override object Deserialize(string key, SerializedObject obj) {
            return obj.GetBoolean(key);
        }

        public override void Serialize(bool value, string name, SerializedObject destination) {
            destination.SetBoolean(name, value);
        }
    }

    public class IntSerializer : Serializer<int> {
        public override object Deserialize(string key, SerializedObject obj) {
            return obj.GetInt(key);
        }

        public override void Serialize(int value, string name, SerializedObject destination) {
            destination.SetInt(name, value);
        }
    }

    public class FloatSerializer : Serializer<float> {
        public override object Deserialize(string key, SerializedObject obj) {
            return obj.GetFloat(key);
        }

        public override void Serialize(float value, string name, SerializedObject destination) {
            destination.SetFloat(name, value);
        }
    }

    public class StringSerializer : Serializer<string> {
        public override object Deserialize(string key, SerializedObject obj) {
            return obj.GetString(key);
        }

        public override void Serialize(string value, string name, SerializedObject destination) {
            destination.SetString(name, value);
        }
    }

    public class ObjectSerializer : Serializer<SerializedObject> {
        public override object Deserialize(string key, SerializedObject obj) {
            return obj.GetObject(key);
        }

        public override void Serialize(SerializedObject value, string name, SerializedObject destination) {
            destination.SetObject(name, value);
        }
    }
}