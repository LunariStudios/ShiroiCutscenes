using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shiroi.Cutscenes.Editor.Config;
using Shiroi.Cutscenes.Editor.Drawers;
using Shiroi.Cutscenes.Editor.Errors;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Editor.Windows;
using Shiroi.Cutscenes.Serialization;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Serialization;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Editor {
    public class MappedToken {
        //Setter things
        private static FieldInfo currentField;

        private static IToken currentToken;

        private static readonly Setter Setter = value => currentField.SetValue(currentToken, value);

        //Const value
        private const float SaturationValue = 0.5F;

        private const float BrightnessValue = 0.7F;
        private const float BrightnessDifference = 0.2F;
        private const float SelectedBrightnessValue = BrightnessValue + BrightnessDifference;

        private static readonly Dictionary<Type, MappedToken> Cache = new Dictionary<Type, MappedToken>();

        public static void Clear() {
            Cache.Clear();
        }


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

        public readonly Color Color;
        public readonly Color SelectedColor;
        public readonly string Label;
        private readonly List<TypeDrawer> drawers = new List<TypeDrawer>();

        public MappedToken(Type type) {
            //Initialize fields
            Label = ObjectNames.NicifyVariableName(type.Name);
            if (Label.EndsWith("Token")) {
                Label = Label.Substring(0, Label.Length - 5);
            }
            SerializedFields = SerializationUtil.GetSerializedMembers(type, true);
            TotalElements = (uint) SerializedFields.Length;
            //Initialize with label
            Height = ShiroiStyles.SingleLineHeight;
            foreach (var field in SerializedFields) {
                var drawer = TypeDrawers.GetDrawerFor(field.FieldType);
                drawers.Add(drawer);
                Height += drawer.GetTotalLines() * ShiroiStyles.SingleLineHeight;
            }
            //Calculate color
            if (Configs.ColorfulTokens) {
                CalculateColor(type, out Color, out SelectedColor);
            } else {
                Color = Color.HSVToRGB(0, 0, BrightnessValue);
                SelectedColor = Color.HSVToRGB(0, 0, SelectedBrightnessValue);
            }
        }

        private static void CalculateColor(Type type, out Color color, out Color selectedColor) {
            var name = type.Name;
            var totalLetters = name.Length;
            var increment = (float) (totalLetters - 1) / 5;
            float r, g, b;
            r = LoadColor(name, 0, increment);
            g = LoadColor(name, 2, increment);
            b = LoadColor(name, 4, increment);
            var typeColor = new Color(r, g, b);
            float h, s, v;
            Color.RGBToHSV(typeColor, out h, out s, out v);
            //Use hsv to calculate brightness
            color = Color.HSVToRGB(h, SaturationValue, BrightnessValue);
            selectedColor = Color.HSVToRGB(h, SaturationValue, SelectedBrightnessValue);
        }

        public FieldInfo[] SerializedFields {
            get;
            private set;
        }

        public float Height {
            get;
            private set;
        }

        public uint TotalElements {
            private set;
            get;
        }

        public delegate void FieldDrawnListener(Rect rect, Cutscene cutscene, CutscenePlayer player, IToken token,
            int tokenIndex, FieldInfo field, int fieldIndex, ref GUIContent fieldLabel);

        public delegate void TokenDrawnListener(Rect rect, Cutscene cutscene, CutscenePlayer player, IToken token,
            int tokenIndex, ref GUIContent tokenLabel);

        public static event FieldDrawnListener OnFieldDrawn;
        public static event TokenDrawnListener OnTokenDrawn;

        public void DrawFields(CutsceneEditor editor, Rect rect, int tokenIndex, IToken token, Cutscene cutscene,
            CutscenePlayer player,
            out bool changed) {
            var labelRect = rect.GetLine(0);
            var content = new GUIContent(string.Format("#{0} - {1}", tokenIndex, Label));
            InvokeOnTokenDrawn(rect, cutscene, player, token, tokenIndex, ref content);
            EditorGUI.LabelField(labelRect, content, ShiroiStyles.Bold);
            changed = false;
            currentToken = token;
            //Start at 1 because label
            var currentLine = 1;
            for (var index = 0; index < SerializedFields.Length; index++) {
                currentField = SerializedFields[index];
                var fieldType = currentField.FieldType;
                var drawer = drawers[index];

                var fieldName = currentField.Name;
                var typeName = fieldType.Name;
                var totalLines = drawer.GetTotalLines();
                var r = rect.GetLine((uint) currentLine, totalLines);
                currentLine += (int) totalLines;
                var fieldLabel = new GUIContent(ObjectNames.NicifyVariableName(fieldName));
                InvokeOnFieldDrawn(r, cutscene, player, token, tokenIndex, currentField, index, ref fieldLabel);
                EditorGUI.BeginChangeCheck();

                drawer.Draw(editor, player, cutscene, r, tokenIndex, fieldLabel,
                    currentField.GetValue(token),
                    fieldType, currentField, Setter);

                if (EditorGUI.EndChangeCheck()) {
                    changed = true;
                }
            }
        }

        protected virtual void InvokeOnFieldDrawn(Rect rect, Cutscene cutscene, CutscenePlayer player, IToken token,
            int tokenindex, FieldInfo field, int fieldindex, ref GUIContent label) {
            var handler = OnFieldDrawn;
            if (handler != null)
                handler(rect, cutscene, player, token, tokenindex, field, fieldindex, ref label);
        }

        private static void InvokeOnTokenDrawn(Rect rect, Cutscene cutscene, CutscenePlayer player, IToken token,
            int tokenindex, ref GUIContent tokenlabel) {
            var handler = OnTokenDrawn;
            if (handler != null)
                handler(rect, cutscene, player, token, tokenindex, ref tokenlabel);
        }
    }
}