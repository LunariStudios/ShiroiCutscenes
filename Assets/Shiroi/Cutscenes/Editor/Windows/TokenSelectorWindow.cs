using System;
using Shiroi.Cutscenes.Editor.Util;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Windows {
    public class TokenSelectorWindow : PopupWindowContent {
        // Label
        // Filter
        public const int BuiltInLines = 2;

        public const string FilterLabel = "Filter";
        public const float WindowWidth = 300;

        public static Vector2 Size {
            get {
                return new Vector2(WindowWidth,
                    (TokenLoader.KnownTokenTypes.Count + BuiltInLines) * ShiroiStyles.SingleLineHeight);
            }
        }

        public TokenSelectorWindow(CutsceneEditor currentEditor) {
            CurrentEditor = currentEditor;
        }

        public CutsceneEditor CurrentEditor {
            get;
            private set;
        }

        public override Vector2 GetWindowSize() {
            return Size;
        }

        private string filter = string.Empty;

        public override void OnGUI(Rect rect) {
            EditorGUI.LabelField(rect.GetLine(0), "Select a token to add");
            EditorGUI.BeginChangeCheck();
            filter = EditorGUI.TextField(rect.GetLine(1), FilterLabel, filter);
            if (EditorGUI.EndChangeCheck()) {
                editorWindow.Repaint();
                return;
            }
            var i = 0;
            foreach (var type in TokenLoader.KnownTokenTypes) {
                if (!string.IsNullOrEmpty(filter) &&
                    !type.Name.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase)) {
                    continue;
                }
                GUI.color = MappedToken.For(type).Color;
                if (GUI.Button(rect.GetLine((uint) (i + BuiltInLines)), type.Name)) {
                    CurrentEditor.AddToken(type);
                }
                i++;
            }
        }
    }
}