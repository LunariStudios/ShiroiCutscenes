using System;
using System.Collections.Generic;
using System.Reflection;
using Shiroi.Cutscenes.Serialization;
using Shiroi.Cutscenes.Tokens;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Editor {
    public class MappedToken {
        private static Dictionary<Type, MappedToken> cache = new Dictionary<Type, MappedToken>();
        public GUIStyle style;
        public GUIStyle selectedStyle;

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
            //Calculate color
            var name = token.GetType().Name;
            var totalLetters = name.Length;
            var increment = totalLetters / 6;
            float r, g, b;
            r = LoadColor(name, 0, increment);
            g = LoadColor(name, 1, increment);
            b = LoadColor(name, 2, increment);
            var normalColor = new Color(r, g, b);
            float h, s, v;
            Color.RGBToHSV(normalColor, out h, out s, out v);
            normalColor = Color.HSVToRGB(h, 0.7F, 0.7F);
            var selectedColor = Color.HSVToRGB(h, 0.7F, 0.9F);
            style = CreateGUIStyle(normalColor);
            selectedStyle = CreateGUIStyle(selectedColor);
        }

        private GUIStyle CreateGUIStyle(Color color) {
            return new GUIStyle(GUI.skin.box) {normal = {background = CreateTexture(2, 2, color)}};
        }

        public static Texture2D CreateTexture(int width, int height, Color color) {
            var pixels = new Color[width * height];
            for (var i = 0; i < pixels.Length; ++i) {
                pixels[i] = color;
            }
            var result = new Texture2D(width, height);
            result.SetPixels(pixels);
            result.Apply();
            return result;
        }

        private static float LoadColor(string str, int offset, int increment) {
            return(float) LoadByte(str, offset, increment) / byte.MaxValue;
        }

        private static byte LoadByte(string str, int offset, int increment) {
            var first = (byte) (str[offset * increment] % 16);
            var second = (byte) (str[(offset + 1) * increment] % 16);
            var ff = (char) (first <= 9 ? '0' + first : 'A' + (first - 10));
            var fs = (char) (second <= 9 ? '0' + second : 'A' + (second - 10));

            var r = string.Format("{0}{1}", ff, fs);
            return byte.Parse(r, System.Globalization.NumberStyles.HexNumber);
        }

        public void DrawFields(Rect rect, IToken token) {
            for (var index = 0; index < SerializedFields.Length; index++) {
                var field = SerializedFields[index];
                var fieldType = field.FieldType;
                DrawField(index + 1, rect, field, fieldType, Type.GetTypeCode(fieldType), token);
            }
        }

        //TODO make drawing api
        private static void DrawField(int index, Rect rect, FieldInfo field, Type fieldType, TypeCode code,
            IToken token) {
            var fieldName = field.Name;
            var r = CutsceneEditor.GetRect(rect, index);
            var value = field.GetValue(token);
            if (fieldType.IsEnum) {
                field.SetValue(token, EditorGUI.EnumPopup(r, fieldName, (Enum) value));
                return;
            }
            var o = value as Object;
            if (o != null) {
                field.SetValue(token, EditorGUI.ObjectField(r, fieldName, o, fieldType, false));
                return;
            }
            if (value is Vector2) {
                field.SetValue(token, EditorGUI.Vector2Field(r, fieldName, (Vector2) value));
                return;
            }
            if (value is Vector3) {
                field.SetValue(token, EditorGUI.Vector3Field(r, fieldName, (Vector3) value));
                return;
            }
            if (value is Vector4) {
                field.SetValue(token, EditorGUI.Vector4Field(r, fieldName, (Vector4) value));
                return;
            }
            if (value is Quaternion) {
                var v = EditorGUI.Vector3Field(r, fieldName, ((Quaternion) value).eulerAngles);
                field.SetValue(token, Quaternion.Euler(v));
                return;
            }
            if (value is Vector2Int) {
                field.SetValue(token, EditorGUI.Vector2IntField(r, fieldName, (Vector2Int) value));
                return;
            }
            if (value is Vector3Int) {
                field.SetValue(token, EditorGUI.Vector3IntField(r, fieldName, (Vector3Int) value));
                return;
            }
            if (value is Bounds) {
                field.SetValue(token, EditorGUI.BoundsField(r, fieldName, (Bounds) value));
                return;
            }
            if (value is BoundsInt) {
                field.SetValue(token, EditorGUI.BoundsIntField(r, fieldName, (BoundsInt) value));
                return;
            }
            if (value is Color) {
                field.SetValue(token, EditorGUI.ColorField(r, fieldName, (Color) value));
                return;
            }
            if (value is AnimationCurve) {
                field.SetValue(token, EditorGUI.CurveField(r, fieldName, (AnimationCurve) value));
                return;
            }
            if (value is LayerMask) {
                field.SetValue(token, EditorGUI.LayerField(r, fieldName, (LayerMask) value));
                return;
            }
            switch (code) {
                case TypeCode.Boolean:
                    field.SetValue(token, EditorGUI.Toggle(r, fieldName, (bool) value));
                    break;
                case TypeCode.Byte:
                    field.SetValue(token, (byte) EditorGUI.IntField(r, fieldName, (byte) value));
                    break;
                case TypeCode.Char:
                    field.SetValue(token, (char) EditorGUI.IntField(r, fieldName, (char) value));
                    break;
                case TypeCode.Decimal:
                    field.SetValue(token,
                        (decimal) EditorGUI.DoubleField(r, fieldName, (double) value));
                    break;
                case TypeCode.Double:
                    field.SetValue(token, EditorGUI.DoubleField(r, fieldName, (double) value));
                    break;
                case TypeCode.Int16:
                    field.SetValue(token, (short) EditorGUI.IntField(r, fieldName, (int) value));
                    break;
                case TypeCode.Int32:
                    field.SetValue(token, EditorGUI.IntField(r, fieldName, (int) value));
                    break;
                case TypeCode.Int64:
                    field.SetValue(token, EditorGUI.LongField(r, fieldName, (long) value));
                    break;
                case TypeCode.SByte:
                    field.SetValue(token, (sbyte) EditorGUI.IntField(r, fieldName, (int) value));
                    break;
                case TypeCode.Single:
                    field.SetValue(token, EditorGUI.FloatField(r, fieldName, (float) value));
                    break;
                case TypeCode.String:
                    field.SetValue(token, EditorGUI.TextField(r, fieldName, (string) value));
                    break;
                case TypeCode.UInt16:
                    field.SetValue(token, (ushort) EditorGUI.IntField(r, fieldName, (int) value));
                    break;
                case TypeCode.UInt32:
                    field.SetValue(token, (uint) EditorGUI.LongField(r, fieldName, (long) value));
                    break;
                case TypeCode.UInt64:
                    field.SetValue(token, (ulong) EditorGUI.LongField(r, fieldName, (long) value));
                    break;
                default:
                    EditorGUI.LabelField(r, "Couldn't find editor for type '" +
                                            fieldType.Name
                                            + "' of field '" +
                                            fieldName
                                            + "'", CutsceneEditor.errorStyle);
                    break;
            }
        }
    }
}