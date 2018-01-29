using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Windows {
    public class TokenSelectorWindow : PopupWindowContent {

        public static Vector2 Size {
            get {
                return new Vector2(200, (TokenLoader.KnownTokenTypes.Count + 1) * EditorGUIUtility.singleLineHeight);
            }
        }

        public TokenSelectorWindow(CutsceneEditor currentEditor) {
            CurrentEditor = currentEditor;
        }
        public CutsceneEditor CurrentEditor { get; private set; }

        public override Vector2 GetWindowSize() {
            return Size;
        }

        public override void OnGUI(Rect rect) {
            EditorGUI.LabelField(CutsceneEditor.GetRect(rect, 0), "Select a token to add");
            for (var i = 0; i < TokenLoader.KnownTokenTypes.Count; i++) {
                var type = TokenLoader.KnownTokenTypes[i];
                GUI.color = MappedToken.For(type).Color;
                if (GUI.Button(CutsceneEditor.GetRect(rect, i + 1), type.Name)) {
                    CurrentEditor.AddToken(type);
                }
            }
        }
    }
}