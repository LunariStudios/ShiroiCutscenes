using System;

namespace Shiroi.Cutscenes.Serialization {
    public class ByteSerializer : Serializer<byte> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return (byte) obj.GetInt(key);
        }

        public override void Serialize(byte value, string name, SerializedObject destination) {
            destination.SetInt(name, value);
        }
    }

    public class EnumSerializer : Serializer {
        public override bool Supports(Type type) {
            return type.IsEnum;
        }

        public override void Serialize(object value, string name, SerializedObject destination) {
            destination.SetInt(name, (int) value);
        }

        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return Enum.GetValues(fieldType).GetValue(obj.GetInt(key));
        }
    }

    public class SignedByteSerializer : Serializer<sbyte> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return (sbyte) obj.GetInt(key);
        }

        public override void Serialize(sbyte value, string name, SerializedObject destination) {
            destination.SetInt(name, value);
        }
    }

    public class ShortSerializer : Serializer<short> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return (short) obj.GetInt(key);
        }

        public override void Serialize(short value, string name, SerializedObject destination) {
            destination.SetInt(name, value);
        }
    }

    public class UnsignedShortSerializer : Serializer<ushort> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return (ushort) obj.GetInt(key);
        }

        public override void Serialize(ushort value, string name, SerializedObject destination) {
            destination.SetInt(name, value);
        }
    }


    public class UnsignedIntSerializer : Serializer<uint> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return (uint) obj.GetInt(key);
        }

        public override void Serialize(uint value, string name, SerializedObject destination) {
            destination.SetInt(name, (int) value);
        }
    }

    public class LongSerializer : Serializer<long> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return (long) obj.GetInt(key);
        }

        public override void Serialize(long value, string name, SerializedObject destination) {
            destination.SetInt(name, (int) value);
        }
    }

    public class UnsignedLongSerializer : Serializer<ulong> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return (ulong) obj.GetInt(key);
        }

        public override void Serialize(ulong value, string name, SerializedObject destination) {
            destination.SetInt(name, (int) value);
        }
    }

    public class DoubleSerializer : Serializer<double> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return (float) obj.GetInt(key);
        }

        public override void Serialize(double value, string name, SerializedObject destination) {
            destination.SetFloat(name, (float) value);
        }
    }


    public class BooleanSerializer : Serializer<bool> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return obj.GetBoolean(key);
        }

        public override void Serialize(bool value, string name, SerializedObject destination) {
            destination.SetBoolean(name, value);
        }
    }

    public class IntSerializer : Serializer<int> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return obj.GetInt(key);
        }

        public override void Serialize(int value, string name, SerializedObject destination) {
            destination.SetInt(name, value);
        }
    }

    public class FloatSerializer : Serializer<float> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return obj.GetFloat(key);
        }

        public override void Serialize(float value, string name, SerializedObject destination) {
            destination.SetFloat(name, value);
        }
    }

    public class StringSerializer : Serializer<string> {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return obj.GetString(key);
        }

        public override void Serialize(string value, string name, SerializedObject destination) {
            destination.SetString(name, value);
        }
    }
}