using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor {
    public class TokenSelectorWindow : EditorWindow {
        public CutsceneEditor CurrentEditor { get; set; }

        private void OnGUI() {
            EditorGUILayout.LabelField("Select a token to add");
            foreach (var type in TokenLoader.knownTokenTypes) {
                if (GUILayout.Button(type.Name)) {
                    CurrentEditor.AddToken(type);
                }
            }
        }
    }
}