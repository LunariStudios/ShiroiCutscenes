using System;
using System.Collections.Generic;
using System.Reflection;
using Shiroi.Cutscenes.Editor.Drawers;
using Shiroi.Cutscenes.Serialization;
using Shiroi.Cutscenes.Tokens;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Editor {
    public class MappedToken {
        private const float SaturationValue = 0.5F;
        private const float BrightnessValue = 0.7F;
        private const float SelectedBrightnessValue = BrightnessValue + 0.2F;

        private static readonly Dictionary<Type, MappedToken> Cache = new Dictionary<Type, MappedToken>();
        private static readonly Dictionary<Type, Setter> SetterCache = new Dictionary<Type, Setter>();

        public static MappedToken For(IToken token) {
            return For(token.GetType());
        }

        public static MappedToken For(Type type) {
            MappedToken t;
            if (Cache.TryGetValue(type, out t)) {
                return t;
            }
            return Cache[type] = new MappedToken(type);
        }

        private static Texture2D CreateTexture(int width, int height, Color color) {
            var pixels = new Color[width * height];
            for (var i = 0; i < pixels.Length; ++i) {
                pixels[i] = color;
            }
            var result = new Texture2D(width, height);
            result.SetPixels(pixels);
            result.Apply();
            return result;
        }

        private static float LoadColor(string str, int offset, float increment) {
            return (float) LoadByte(str, offset, increment) / byte.MaxValue;
        }

        private static byte LoadByte(string str, int offset, float increment) {
            var indexA = (int) (offset * increment);
            var indexB = (int) ((offset + 1) * increment);
            var first = (byte) (str[indexA] % 16);
            var second = (byte) (str[indexB] % 16);
            var ff = (char) (first <= 9 ? '0' + first : 'A' + (first - 10));
            var fs = (char) (second <= 9 ? '0' + second : 'A' + (second - 10));
            var r = string.Format("{0}{1}", ff, fs);
            return byte.Parse(r, System.Globalization.NumberStyles.HexNumber);
        }

        public GUIStyle Style;

        public GUIStyle SelectedStyle;

        public Color Color;
        public Color SelectedColor;
        public GUIContent Label;

        public MappedToken(Type type) {
            SerializedFields = SerializationUtil.GetSerializedMembers(type);
            TotalElements = (uint) SerializedFields.Length;
            Label = new GUIContent(type.Name);
            //Calculate color
            var name = type.Name;
            var totalLetters = name.Length;
            var increment = (float) (totalLetters - 1) / 5;
            float r, g, b;
            //load colors from string
            r = LoadColor(name, 0, increment);
            g = LoadColor(name, 2, increment);
            b = LoadColor(name, 4, increment);
            Color = new Color(r, g, b);
            float h, s, v;
            Color.RGBToHSV(Color, out h, out s, out v);
            Color = Color.HSVToRGB(h, SaturationValue, BrightnessValue);
            SelectedColor = Color.HSVToRGB(h, SaturationValue, SelectedBrightnessValue);
            Style = CreateGUIStyle(Color);
            SelectedStyle = CreateGUIStyle(SelectedColor);
        }

        public FieldInfo[] SerializedFields { get; private set; }

        public float Height {
            get { return (TotalElements + 1) * EditorGUIUtility.singleLineHeight; }
        }

        public uint TotalElements { private set; get; }


        private GUIStyle CreateGUIStyle(Color color) {
            return new GUIStyle(GUI.skin.box) {normal = {background = CreateTexture(2, 2, color)}};
        }

        private static FieldInfo currentField;
        private static IToken currentToken;
        private Setter s = value => currentField.SetValue(currentToken, value);

        public void DrawFields(Rect rect, int tokenIndex, IToken token, Cutscene cutscene, CutscenePlayer player, out bool changed) {
            changed = false;
            currentToken = token;
            for (var index = 0; index < SerializedFields.Length; index++) {
                currentField = SerializedFields[index];
                var fieldType = currentField.FieldType;
                var drawer = TypeDrawers.GetDrawerFor(fieldType);
                var fieldName = currentField.Name;
                var typeName = fieldType.Name;
                var r = CutsceneEditor.GetRect(rect, index + 1);
                if (drawer == null) {
                    EditorGUI.LabelField(r,
                        string.Format("Couldn't find drawer for field '{0}' of type '{1}'", fieldName, typeName));
                    continue;
                }
                EditorGUI.BeginChangeCheck();
                drawer.Draw(player, cutscene, r, tokenIndex, ObjectNames.NicifyVariableName(fieldName),
                    currentField.GetValue(token),
                    fieldType, currentField, s);
                if (EditorGUI.EndChangeCheck()) {
                    changed = true;
                }
            }
        }
    }
}