using Shiroi.Cutscenes.Editor.Util;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor {
    public static class ShiroiEditorUtil {
        public static void DrawReferencesLayout(CutscenePlayer player) {
            var references = player.References;
            var total = references.Count;
            if (total == 0) {
                EditorGUILayout.LabelField(CutscenePlayerEditor.NoReferences, ShiroiStyles.Bold);
                return;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Format("There are a total of {0} references.", total), ShiroiStyles.Bold);
            if (GUILayout.Button(CutscenePlayerEditor.Clear)) {
                player.ClearReferences();
                return;
            }
            EditorGUILayout.EndHorizontal();
            
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
                EditorGUI.LabelField(msgRect, string.Format("{0} @ {1} ({2} uses)", obj.name, reference.Id, reference.TotalUses));
            }
        }
    }
}