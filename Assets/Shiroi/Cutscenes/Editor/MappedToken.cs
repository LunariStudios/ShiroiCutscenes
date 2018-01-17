using System;
using System.Collections.Generic;
using System.Reflection;
using Shiroi.Cutscenes.Serialization;
using Shiroi.Cutscenes.Tokens;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor {
    public class MappedToken {
        private static Dictionary<Type, MappedToken> cache = new Dictionary<Type, MappedToken>();

        public static MappedToken For(IToken token) {
            MappedToken t;
            var type = token.GetType();
            if (cache.TryGetValue(type, out t)) {
                return t;
            }
            return cache[type] = new MappedToken(token);
        }

        public FieldInfo[] SerializedFields { get; private set; }

        public float Height {
            get { return (TotalElements + 1) * EditorGUIUtility.singleLineHeight; }
        }

        public uint TotalElements { private set; get; }

        public MappedToken(IToken token) {
            SerializedFields = SerializationUtil.GetSerializedMembers(token);
            TotalElements = (uint) SerializedFields.Length;
        }

        public void DrawFields(Rect rect, IToken token) {
            for (var index = 0; index < SerializedFields.Length; index++) {
                var field = SerializedFields[index];
                DrawField(index + 1, rect, field, Type.GetTypeCode(field.FieldType), token);
            }
        }

        private void DrawField(int index, Rect rect, FieldInfo field, TypeCode code, IToken token) {
            var fieldName = field.Name;
            var r = CutsceneEditor.GetRect(rect, index);
            if (field.FieldType.IsEnum) {
                field.SetValue(token, EditorGUI.EnumPopup(r, fieldName, (Enum) field.GetValue(token)));
                return;
            }
            switch (code) {
                case TypeCode.Boolean:
                    field.SetValue(token, EditorGUI.Toggle(r, fieldName, (bool) field.GetValue(token)));
                    break;
                case TypeCode.Byte:
                    field.SetValue(token, (byte) EditorGUI.IntField(r, fieldName, (byte) field.GetValue(token)));
                    break;
                case TypeCode.Char:
                    field.SetValue(token, (char) EditorGUI.IntField(r, fieldName, (char) field.GetValue(token)));
                    break;
                case TypeCode.Decimal:
                    field.SetValue(token,
                        (decimal) EditorGUI.DoubleField(r, fieldName, (double) field.GetValue(token)));
                    break;
                case TypeCode.Double:
                    field.SetValue(token, EditorGUI.DoubleField(r, fieldName, (double) field.GetValue(token)));
                    break;
                case TypeCode.Int16:
                    field.SetValue(token, (short) EditorGUI.IntField(r, fieldName, (int) field.GetValue(token)));
                    break;
                case TypeCode.Int32:
                    field.SetValue(token, EditorGUI.IntField(r, fieldName, (int) field.GetValue(token)));
                    break;
                case TypeCode.Int64:
                    field.SetValue(token, EditorGUI.LongField(r, fieldName, (long) field.GetValue(token)));
                    break;
                case TypeCode.SByte:
                    field.SetValue(token, (sbyte) EditorGUI.IntField(r, fieldName, (int) field.GetValue(token)));
                    break;
                case TypeCode.Single:
                    field.SetValue(token, EditorGUI.FloatField(r, fieldName, (float) field.GetValue(token)));
                    break;
                case TypeCode.String:
                    field.SetValue(token, EditorGUI.TextField(r, fieldName, (string) field.GetValue(token)));
                    break;
                case TypeCode.UInt16:
                    field.SetValue(token, (ushort) EditorGUI.IntField(r, fieldName, (int) field.GetValue(token)));
                    break;
                case TypeCode.UInt32:
                    field.SetValue(token, (uint) EditorGUI.LongField(r, fieldName, (long) field.GetValue(token)));
                    break;
                case TypeCode.UInt64:
                    field.SetValue(token, (ulong) EditorGUI.LongField(r, fieldName, (long) field.GetValue(token)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("code", code, null);
            }
        }
    }
}