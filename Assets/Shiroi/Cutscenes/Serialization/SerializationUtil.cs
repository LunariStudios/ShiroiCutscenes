using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Shiroi.Cutscenes.Serialization {
    public static class SerializationUtil {
        public static FieldInfo[] GetSerializedMembers(object obj) {
            var members = obj.GetType().GetFields();
            var result = new List<FieldInfo>();
            foreach (var memberInfo in members) {
                if (ShouldSerialize(memberInfo)) {
                    result.Add(memberInfo);
                }
            }
            return result.ToArray();
        }

        public static SerializedMember[] Serialize(object obj) {
            var members = obj.GetType().GetFields();
            var result = new List<SerializedMember>();
            foreach (var memberInfo in members) {
                var serialized = TrySerialize(memberInfo, obj);
                if (serialized == null) {
                    continue;
                }
                result.Add(serialized);
            }
            return result.ToArray();
        }

        private static SerializedMember TrySerialize(FieldInfo memberInfo, object o) {
            if (!ShouldSerialize(memberInfo)) {
                return null;
            }
            var fieldName = memberInfo.Name;
            var fieldType = memberInfo.FieldType;
            var value = memberInfo.GetValue(o);
            switch (GetSerializedMemberType(fieldType)) {
                case SerializedMemberType.Boolean:
                    return new SerializedBoolean(fieldName, (bool) value);
                case SerializedMemberType.Byte:
                    return new SerializedByte(fieldName, (byte) value);
                case SerializedMemberType.SByte:
                    return new SerializedSignedByte(fieldName, (sbyte) value);
                case SerializedMemberType.Short:
                    return new SerializedShort(fieldName, (short) value);
                case SerializedMemberType.UShort:
                    return new SerializedUShort(fieldName, (ushort) value);
                case SerializedMemberType.Int:
                    return new SerializedInt(fieldName, (int) value);
                case SerializedMemberType.UInt:
                    return new SerializedUInt(fieldName, (uint) value);
                case SerializedMemberType.String:
                    return new SerializedString(fieldName, (string) value);
                case SerializedMemberType.Char:
                    return new SerializedChar(fieldName, (char) value);
                case SerializedMemberType.Long:
                    return new SerializedLong(fieldName, (long) value);
                case SerializedMemberType.ULong:
                    return new SerializedULong(fieldName, (ulong) value);
                case SerializedMemberType.Float:
                    return new SerializedFloat(fieldName, (float) value);
                case SerializedMemberType.Double:
                    return new SerializedDouble(fieldName, (double) value);
                case SerializedMemberType.Decimal:
                    return new SerializedDecimal(fieldName, (decimal) value);
                case SerializedMemberType.Complex:
                    return new SerializedComplex(fieldName, value);
            }
            throw new ArgumentOutOfRangeException();
        }

        private static SerializedMemberType GetSerializedMemberType(Type fieldType) {
            var typeCode = Type.GetTypeCode(fieldType);
            switch (typeCode) {
                case TypeCode.Boolean:
                    return SerializedMemberType.Boolean;
                case TypeCode.Byte:
                    return SerializedMemberType.Byte;
                case TypeCode.Char:
                    return SerializedMemberType.Char;
                case TypeCode.Decimal:
                    return SerializedMemberType.Decimal;
                case TypeCode.Single:
                    return SerializedMemberType.Float;
                case TypeCode.Double:
                    return SerializedMemberType.Double;
                case TypeCode.Int16:
                    return SerializedMemberType.Short;
                case TypeCode.Int32:
                    return SerializedMemberType.Int;
                case TypeCode.Int64:
                    return SerializedMemberType.Long;
                case TypeCode.SByte:
                    return SerializedMemberType.SByte;
                case TypeCode.String:
                    return SerializedMemberType.String;
                case TypeCode.UInt16:
                    return SerializedMemberType.UShort;
                case TypeCode.UInt32:
                    return SerializedMemberType.UInt;
                case TypeCode.UInt64:
                    return SerializedMemberType.ULong;
                case TypeCode.Object:
                    return SerializedMemberType.Complex;
            }
            throw new ArgumentOutOfRangeException("fieldType");
        }

        private static bool ShouldSerialize(FieldInfo memberInfo) {
            return !memberInfo.IsPrivate || Attribute.GetCustomAttribute(memberInfo, typeof(SerializeField)) != null;
        }
    }
}