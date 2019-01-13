using System;
using Lunari.Tsuki;
using Shiroi.Cutscenes.Communication;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Util;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Communication {
    [CustomPropertyDrawer(typeof(Output), true)]
    public class OutputDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent l) {
            var output = property.FindInstanceWithin<Output>();
            var value = output.Name;
            Draw(l, ref value, position, output.GetOutputType());
            output.Name = value;
        }

        public const float ColorSquareWidth = 16f;
        public static readonly Vector2 ColorSquareSize = new Vector2(ColorSquareWidth, ColorSquareWidth);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }


        public static void Draw(GUIContent label, ref string name, Rect position, Type type) {
            label.text += $" (Output of: {type.GetNiceName()})";
            name = EditorGUI.TextField(position, label, name);
            var iconRect = position;
            iconRect.xMin += EditorGUIUtility.labelWidth + EditorStyles.label.CalcSize(new GUIContent(name)).x;
            iconRect.width = iconRect.height;
            
            GUI.DrawTexture(iconRect.Padding(IconPadding), ShiroiEditorUtil.GetIconFor(type));
            if (name.IsNullOrEmpty()) {
                name = $"{type.Name}_output";
            }

            var xOffset = EditorStyles.label.CalcSize(label).x;
            var colorRect = position;
            colorRect.xMin += xOffset;
            colorRect.size = ColorSquareSize;
            EditorGUI.DrawRect(colorRect.Padding(IconPadding), CommunicationDeviceUtility.GetColorFromName(name));
        }

        public static float IconPadding = 2;
    }
}