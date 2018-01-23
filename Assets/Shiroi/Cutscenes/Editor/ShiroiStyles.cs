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

        public static readonly Color PlayerConfigColor = new Color(0.26f, 0.63f, 0.28f);
        public static readonly Color NoBoundPlayerColor = new Color(0.9f, 0.22f, 0.21f);
        public static readonly GUILayoutOption IconHeightOption = GUILayout.Height(IconSize);
        public static readonly GUILayoutOption IconWidthOption = GUILayout.Width(IconSize);

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
            PlayerConfig = CreateGUIStyle(PlayerConfigColor);
            NoBoundPlayer = CreateGUIStyle(NoBoundPlayerColor);
        }

        public static GUIStyle PlayerConfig { get; private set; }
        public static GUIStyle NoBoundPlayer { get; private set; }

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