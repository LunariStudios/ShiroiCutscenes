using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor {
    [InitializeOnLoad]
    public static class ShiroiStyles {
        public const int KaomojiFontSize = 100;
        public const int KaomojiVerticalBorder = 50;
        public const int KaomojiHorizontalBorder = 50;
        public const int IconSize = 24;
        public const float SingleLineHeight = 16;

        public static readonly GUILayoutOption IconHeightOption = GUILayout.Height(IconSize);
        public static readonly GUILayoutOption IconWidthOption = GUILayout.Width(IconSize);

        static ShiroiStyles() {
            var kaomojiOffset = new RectOffset(
                KaomojiHorizontalBorder,
                KaomojiHorizontalBorder,
                KaomojiVerticalBorder,
                KaomojiVerticalBorder
            );
            Base = new GUIStyle {
                alignment = TextAnchor.MiddleCenter
            };
            Bold = new GUIStyle {
                fontStyle = FontStyle.Bold
            };
            Header = new GUIStyle(Base) {
                fontStyle = FontStyle.Bold
            };
            Error = new GUIStyle(Base) {
                fontStyle = FontStyle.Bold,
            };
            Kaomoji = new GUIStyle(Error) {
                fontSize = KaomojiFontSize,
                margin = kaomojiOffset,
            };
        }

        public static GUIStyle Checkbox1 { get; private set; }
        public static GUIStyle Checkbox2 { get; private set; }
        public static GUIStyle Base { get; private set; }
        public static GUIStyle Header { get; private set; }
        public static GUIStyle Error { get; private set; }
        public static GUIStyle Kaomoji { get; private set; }
        public static GUIStyle Bold { get; private set; }
        public static readonly GUILayoutOption ExpandWidthOption = GUILayout.ExpandWidth(true);
    }
}