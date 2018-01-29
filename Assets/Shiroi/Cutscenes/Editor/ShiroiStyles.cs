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

        public static readonly Color ErrorBackgroundColor = new Color(0.9f, 0.22f, 0.21f);
        public static readonly Color DefaultBackgroundColor = new Color(0.6f, 0.6f, 0.6f);

        public static GUIStyle CreateGUIStyle(Color color) {
            return new GUIStyle {normal = {background = CreateTexture(1, 1, color)}};
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

        static ShiroiStyles() {
            var kaomojiOffset = new RectOffset(
                KaomojiHorizontalBorder,
                KaomojiHorizontalBorder,
                KaomojiVerticalBorder,
                KaomojiVerticalBorder
            );
            ErrorBackground = CreateGUIStyle(ErrorBackgroundColor);
            DefaultBackground = CreateGUIStyle(DefaultBackgroundColor);
            Base = new GUIStyle {
                alignment = TextAnchor.MiddleCenter
            };
            Bold = new GUIStyle {
                fontStyle = FontStyle.Bold
            };
            Header = new GUIStyle(Base) {
                fontStyle = FontStyle.Bold
            };
            Error = new GUIStyle(ErrorBackground) {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
            };
            Kaomoji = new GUIStyle(Header) {
                fontSize = KaomojiFontSize,
                margin = kaomojiOffset,
            };
        }

        public static GUIStyle ErrorBackground { get; private set; }

        public static GUIStyle Base { get; private set; }
        public static GUIStyle Header { get; private set; }
        public static GUIStyle Error { get; private set; }
        public static GUIStyle Kaomoji { get; private set; }
        public static GUIStyle Bold { get; private set; }
        public static GUIStyle DefaultBackground { get; private set; }

        public static readonly GUILayoutOption ExpandWidthOption = GUILayout.ExpandWidth(true);
    }
}