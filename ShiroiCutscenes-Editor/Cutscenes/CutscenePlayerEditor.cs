using UnityEditor;

namespace Shiroi.Cutscenes.Editor.Cutscenes {
    [CustomEditor(typeof(CutscenePlayer))]
    public partial class CutscenePlayerEditor : UnityEditor.Editor {
        public CutscenePlayer Player {
            get;
            private set;
        }

        private void OnEnable() {
            Player = (CutscenePlayer) target;
        }

        public override void OnInspectorGUI() {
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            DrawLayout(Player, skin);
        }
    }
}