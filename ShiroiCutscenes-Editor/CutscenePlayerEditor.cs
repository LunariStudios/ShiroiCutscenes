using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor {
    [CustomEditor(typeof(CutscenePlayer))]
    public class CutscenePlayerEditor : UnityEditor.Editor {
        private CutscenePlayer player;
        public static readonly GUIContent NoReferences = new GUIContent("There are no exposed references saved on this player.");
        public static readonly GUIContent Clear = new GUIContent("Clear References");

        private void OnEnable() {
            player = (CutscenePlayer) target;
        }

        public override void OnInspectorGUI() {
            ShiroiEditorUtil.DrawReferencesLayout(player);
        }
    }
}