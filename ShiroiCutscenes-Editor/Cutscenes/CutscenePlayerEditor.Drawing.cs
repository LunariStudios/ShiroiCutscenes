using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Cutscenes {
    public partial class CutscenePlayerEditor {
        public static void DrawLayout(CutscenePlayer player, GUISkin skin) {
            foreach (var reference in player.References) {
                DrawReferences(reference, skin);
            }
        }

        private static void DrawReferences(CutscenePlayer.SceneReference reference, GUISkin skin) {
            EditorGUILayout.BeginHorizontal(skin.box);
            var obj = reference.Object;
            var content = EditorGUIUtility.ObjectContent(null, obj.GetType());
            GUILayout.Box(content, skin.box,
                GUILayout.Height(ShiroiCutscenesEditorConstants.IconSize),
                GUILayout.Width(ShiroiCutscenesEditorConstants.IconSize)
            );
            var msg = string.Format("{0} @ {1}", obj.name, reference.Id);
            GUILayout.Label(msg);
            EditorGUILayout.EndHorizontal();
        }
    }
}