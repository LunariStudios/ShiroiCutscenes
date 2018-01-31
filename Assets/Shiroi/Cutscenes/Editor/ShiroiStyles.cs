using System;
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

        public static readonly Color ErrorBackgroundColor = new Color(0.9f, 0.22f, 0.21f);
        public static readonly Color MediumErrorBackgroundColor = new Color(0.99f, 0.85f, 0.21f);
        public static readonly Color LowErrorBackgroundColor = new Color(0.33f, 0.43f, 0.48f);
        public static readonly Color DefaultBackgroundColor = new Color(0.6f, 0.6f, 0.6f);

        //Errors
        public const string ErrorIconName = "d_console.erroricon.png";

        public const string WarningIconName = "d_console.warnicon.png";
        public const string InfoIconName = "d_console.infoicon.png";

        public static Texture2D ErrorIcon { get; private set; }
        public static Texture2D WarningIcon { get; private set; }
        public static Texture2D InfoIcon { get; private set; }
        public static GUIContent ErrorContent { get; private set; }
        public static GUIContent WarningContent { get; private set; }
        public static GUIContent InfoContent { get; private set; }

        public static Color GetColor(ErrorLevel level) {
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
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
            };
            Error = new GUIStyle(HeaderCenter) {
                normal = {
                    background = CreateTexture(ErrorBackgroundColor)
                }
            };
            Warning = new GUIStyle(HeaderCenter) {
                normal = {
                    background = CreateTexture(MediumErrorBackgroundColor)
                }
            };
            Info = new GUIStyle(HeaderCenter) {
                normal = {
                    background = CreateTexture(LowErrorBackgroundColor)
                }
            };
            Kaomoji = new GUIStyle(Header) {
                fontSize = KaomojiFontSize,
                margin = kaomojiOffset,
            };
        }

        private static Texture2D CreateTexture(Color color) {
            return CreateTexture(1, 1, color);
        }

        private static GUIContent CreateContent(Texture errorIcon) {
            return new GUIContent {
                image = errorIcon
            };
        }

        private static Texture2D LoadIcon(string path) {
            return EditorGUIUtility.Load("icons/" + path) as Texture2D;
        }

        public static GUIStyle Base { get; private set; }
        public static GUIStyle Header { get; private set; }
        public static GUIStyle HeaderCenter { get; private set; }
        public static GUIStyle Error { get; private set; }
        public static GUIStyle Kaomoji { get; private set; }
        public static GUIStyle Bold { get; private set; }
        public static GUIStyle DefaultBackground { get; private set; }
        public static GUIStyle Warning { get; private set; }
        public static GUIStyle Info { get; private set; }

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
    }
}