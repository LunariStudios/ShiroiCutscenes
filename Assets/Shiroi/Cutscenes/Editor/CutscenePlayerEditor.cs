using Shiroi.Cutscenes.Editor.Util;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor {
    [CustomEditor(typeof(CutscenePlayer))]
    public class CutscenePlayerEditor : UnityEditor.Editor {
        private CutscenePlayer player;
        public static readonly GUIContent NoReferences = new GUIContent("There are no exposed references on this player.");
        public static readonly GUIContent Clear = new GUIContent("Clear References");

        private void OnEnable() {
            player = (CutscenePlayer) target;
        }

        public override void OnInspectorGUI() {
            var references = player.References;
            var total = references.Count;
            if (total == 0) {
                EditorGUILayout.LabelField(NoReferences, ShiroiStyles.Bold);
                return;
            }
            if (GUILayout.Button(Clear)) {
                player.ClearReferences();
                return;
            }
            EditorGUILayout.LabelField(string.Format("There are a total of {0} references.", total), ShiroiStyles.Bold);
            const int iconSize = ShiroiStyles.IconSize;
            for (var i = 0; i < references.Count; i++) {
                var reference = references[i];
                var obj = reference.Object;
                var futureRect = GUILayoutUtility.GetRect(0, iconSize, ShiroiStyles.ExpandWidthOption);
                var content = EditorGUIUtility.ObjectContent(null, obj.GetType());
                content.text = null;

                var iconRect = futureRect.SubRect(iconSize, iconSize);
                var msgRect = futureRect.SubRect(futureRect.width - iconSize, iconSize, iconSize);
                GUI.Box(iconRect, content);
                EditorGUI.LabelField(msgRect, string.Format("{0} @ {1}", obj.name, reference.Id));
            }
        }
    }
}