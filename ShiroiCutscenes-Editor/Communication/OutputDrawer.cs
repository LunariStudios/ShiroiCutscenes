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
            var label = new GUIContent(l);
            label.text += $" (Output of: {output.GetOutputType().GetNiceName()})";

            output.Name = EditorGUI.TextField(position, label, output.Name);
            if (output.Name.IsNullOrEmpty()) {
                output.Name = $"{output.GetOutputType().Name}_output";
            }

            var xOffset = EditorStyles.label.CalcSize(label).x;
            var colorRect = position;
            colorRect.xMin += xOffset;
            colorRect.size = ColorSquareSize;
            EditorGUI.DrawRect(colorRect, output.GetColorFromName());
        }

        public const float ColorSquareWidth = 16f;
        public static readonly Vector2 ColorSquareSize = new Vector2(ColorSquareWidth, ColorSquareWidth);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}