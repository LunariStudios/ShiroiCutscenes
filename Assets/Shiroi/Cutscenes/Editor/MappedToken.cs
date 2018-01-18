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
        private static Dictionary<Type, MappedToken> cache = new Dictionary<Type, MappedToken>();
        private static Dictionary<Type, Setter> setterCache = new Dictionary<Type, Setter>();
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

        public void DrawFields(Rect rect, IToken token, Cutscene cutscene) {
            for (var index = 0; index < SerializedFields.Length; index++) {
                var field = SerializedFields[index];
                var fieldType = field.FieldType;
                var setter = GetSetterFor(fieldType, token, field);
                var drawer = TypeDrawers.GetDrawerFor(fieldType);
                var fieldName = field.Name;
                var typeName = fieldType.Name;
                var r = CutsceneEditor.GetRect(rect, index + 1);
                if (drawer == null) {
                    EditorGUI.LabelField(r,
                        string.Format("Couldn't find drawer for field '{0}' of type '{1}'", fieldName, typeName));
                    continue;
                }
                EditorGUI.BeginChangeCheck();
                drawer.Draw(r, fieldName, field.GetValue(token), fieldType, setter);
                if (EditorGUI.EndChangeCheck()) {
                    EditorUtility.SetDirty(cutscene);
                }
            }
        }

        private static Setter GetSetterFor(Type fieldType, IToken token, FieldInfo info) {
            if (setterCache.ContainsKey(fieldType)) {
                return setterCache[fieldType];
            }
            Setter s = value => info.SetValue(token, value);
            setterCache[fieldType] = s;
            return s;
        }
    }
}