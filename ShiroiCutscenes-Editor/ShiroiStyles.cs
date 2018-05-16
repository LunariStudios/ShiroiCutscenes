using System;
using Shiroi.Cutscenes.Editor.Config;
using Shiroi.Cutscenes.Editor.Errors;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor {
    [InitializeOnLoad]
    public static class ShiroiStyles {
        public const int KaomojiFontSize = 100;
        public const int KaomojiVerticalBorder = 50;
        public const int KaomojiHorizontalBorder = 50;
        public const int IconSize = 24;
        public const float SpaceHeight = 10F;

        public const float SingleLineHeight = 16;
        public const float DefaultAlpha = 0.7F;
        public static readonly Color ErrorBackgroundColor = new Color(0.9f, 0.22f, 0.21f);
        public static readonly Color MediumErrorBackgroundColor = new Color(0.99f, 0.85f, 0.21f);
        public static readonly Color LowErrorBackgroundColor = new Color(0.33f, 0.43f, 0.48f);
        public static readonly Color DefaultBackgroundColor = new Color(0.6f, 0.6f, 0.6f);

        //Errors
        public const string ErrorIconName = "d_console.erroricon.png";

        public const string WarningIconName = "d_console.warnicon.png";
        public const string InfoIconName = "d_console.infoicon.png";

        public static Texture2D ErrorIcon {
            get;
            private set;
        }

        public static Texture2D WarningIcon {
            get;
            private set;
        }

        public static Texture2D InfoIcon {
            get;
            private set;
        }

        public static GUIContent ErrorContent {
            get;
            private set;
        }

        public static GUIContent WarningContent {
            get;
            private set;
        }

        public static GUIContent InfoContent {
            get;
            private set;
        }

        public static Color GetColor(ErrorLevel level) {
            if (!Configs.ErrorColors) {
                return DefaultBackgroundColor;
            }
            switch (level) {
                case ErrorLevel.Low:
                    return LowErrorBackgroundColor;
                case ErrorLevel.Medium:
                    return MediumErrorBackgroundColor;
                case ErrorLevel.High:
                    return ErrorBackgroundColor;
                default:
                    throw new ArgumentOutOfRangeException("level", level, null);
            }
        }

        public static Texture2D GetIcon(ErrorLevel level) {
            switch (level) {
                case ErrorLevel.Low:
                    return InfoIcon;
                case ErrorLevel.Medium:
                    return WarningIcon;
                case ErrorLevel.High:
                    return ErrorIcon;
                default:
                    throw new ArgumentOutOfRangeException("level", level, null);
            }
        }

        public static GUIContent GetErrorLabel(ErrorLevel level, string label) {
            return new GUIContent(GetContent(level)) {
                text = label
            };
        }

        public static GUIContent GetContent(ErrorLevel level) {
            switch (level) {
                case ErrorLevel.Low:
                    return InfoContent;
                case ErrorLevel.Medium:
                    return WarningContent;
                case ErrorLevel.High:
                    return ErrorContent;
                default:
                    throw new ArgumentOutOfRangeException("level", level, null);
            }
        }

        public static GUIStyle CreateGUIStyle(Color color) {
            return new GUIStyle {normal = {background = CreateTexture(color)}};
        }

        public static Texture2D CreateTexture(Color color) {
            return CreateTexture(1, 1, color);
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
            Reload();
        }


        private static GUIContent CreateContent(Texture errorIcon) {
            var icon = new GUIContent();
            if (Configs.ErrorIcons) {
                icon.image = errorIcon;
            }
            return icon;
        }

        private static Texture2D LoadIcon(string path) {
            if (!Configs.ErrorIcons) {
                return CreateTexture(1,1, Color.clear);
            }
            return EditorGUIUtility.Load("icons/" + path) as Texture2D;
        }

        public static GUIStyle Base {
            get;
            private set;
        }
        
        public static GUIStyle Header {
            get;
            private set;
        }

        public static GUIStyle HeaderCenter {
            get;
            private set;
        }

        public static GUIStyle Error {
            get;
            private set;
        }

        public static GUIStyle Kaomoji {
            get;
            private set;
        }

        public static GUIStyle Bold {
            get;
            private set;
        }

        public static GUIStyle DefaultBackground {
            get;
            private set;
        }

        public static GUIStyle Warning {
            get;
            private set;
        }

        public static GUIStyle Info {
            get;
            private set;
        }

        public static readonly GUILayoutOption ExpandWidthOption = GUILayout.ExpandWidth(true);

        public static GUIStyle GetErrorStyle(ErrorLevel max) {
            switch (max) {
                case ErrorLevel.Low:
                    return Info;
                case ErrorLevel.Medium:
                    return Warning;
                case ErrorLevel.High:
                    return Error;
                default:
                    throw new ArgumentOutOfRangeException("max", max, null);
            }
        }

        public static void Reload() {
            ErrorIcon = LoadIcon(ErrorIconName);
            WarningIcon = LoadIcon(WarningIconName);
            InfoIcon = LoadIcon(InfoIconName);

            ErrorContent = CreateContent(ErrorIcon);
            WarningContent = CreateContent(WarningIcon);
            InfoContent = CreateContent(InfoIcon);

            var kaomojiOffset = new RectOffset(
                KaomojiHorizontalBorder,
                KaomojiHorizontalBorder,
                KaomojiVerticalBorder,
                KaomojiVerticalBorder
            );
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
            HeaderCenter = new GUIStyle(Header) {
                fontStyle = FontStyle.Bold,
            };
            Error = CreateErrorStyle(ErrorBackgroundColor);
            Warning = CreateErrorStyle(MediumErrorBackgroundColor);
            Info = CreateErrorStyle(LowErrorBackgroundColor);
            Kaomoji = new GUIStyle(Header) {
                fontSize = KaomojiFontSize,
                margin = kaomojiOffset,
            };
            LoadMessages();
        }

        private static GUIStyle CreateErrorStyle(Color bgColor) {
            var style = new GUIStyle(HeaderCenter);
            if ((bool) Configs.ErrorColors) {
                style.normal.background = CreateTexture(bgColor);
            } else {
                style.normal.background = DefaultBackground.normal.background;
            }
            return style;
        }

        private static void LoadMessages() {
            PlayerHeaderError = new GUIContent(PlayerHeader);
            if (Configs.ErrorIcons) {
                PlayerHeaderError.image = ErrorIcon;
            }
        }

        public const float FuturesHeaderLines = 2.5F;

        public static readonly GUIContent ClearCutscene =
            new GUIContent("Clear cutscene", "Removes all tokens");

        public static readonly GUIContent FuturesStats =
            new GUIContent("Futures Stats", "All of the info on your futures is listed below");

        public static readonly GUIContent PlayerHeader =
            new GUIContent("Player Settings", "All of the info on your Cutscene Player is listed below");

        public static GUIContent PlayerHeaderError {
            get;
            private set;
        }

        public const string NullSupportedMessage = "Please assign it or annotate the field as NullSupported.";

        public static readonly GUIContent PlayerContent =
            new GUIContent("Bound player", "The player that will store the references to the cutscene's scene objects");

        public static readonly GUIContent AddTokenContent =
            new GUIContent("Choose your flavour", "Adds a token to the cutscene");

        public static readonly GUIContent NoTokenContent =
            new GUIContent("There aren't any tokens defined!", "There are no tokens in the cutscene for you to edit.");

        public static readonly string[] Kaomojis = {
            ":(",
            "(^-^*)",
            "(;-;)",
            "(o^^)o",
            "(>_<)",
            "\\(^Д^)/",
            "(≥o≤)",
            "(·.·)",
            "╯°□°）╯︵ ┻━┻"
        };
    }
}