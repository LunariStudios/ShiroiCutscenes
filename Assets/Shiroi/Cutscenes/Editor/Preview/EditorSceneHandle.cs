using Shiroi.Cutscenes.Preview;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Preview {
    public class EditorSceneHandle : ISceneHandle {
        public static readonly EditorSceneHandle Instance = new EditorSceneHandle();
        public static readonly Color LabelBackgroundColor = new Color(.1F, .1F, .1F);
        public static readonly Color LabelFontColor = Color.white;

        public static readonly GUIStyle LabelDefaultStyle = new GUIStyle {
            normal = {
                background = ShiroiStyles.CreateTexture(LabelBackgroundColor),
                textColor = LabelFontColor
            }
        };

        private EditorSceneHandle() { }

        public void Label(Vector3 position, string label) {
            Label(position, label, LabelDefaultStyle);
        }

        public void Label(Vector3 position, string label, GUIStyle style) {
            Handles.Label(position, label, style);
        }

        public Vector3 PositionHandle(Vector3 position) {
            return PositionHandle(position, Quaternion.identity);
        }

        public Vector3 PositionHandle(Vector3 position, string label) {
            return PositionHandle(position, Quaternion.identity, label);
        }

        public Vector3 PositionHandle(Vector3 position, Quaternion rotation) {
            return Handles.PositionHandle(position, rotation);
        }

        public Vector3 PositionHandle(Vector3 position, Quaternion rotation, string label) {
            Label(position, label);
            return PositionHandle(position, rotation);
        }

        public Quaternion RotationHandle(Vector3 position, Quaternion previousRotation) {
            return Handles.RotationHandle(previousRotation, position);
        }

        public Quaternion RotationHandle(Vector3 position, Quaternion previousRotation, string label) {
            Label(position, label);
            return RotationHandle(position, previousRotation);
        }

        public Vector3 ScaleHandle(Vector3 previousScale, Vector3 position, float size = 1) {
            return ScaleHandle(previousScale, position, Quaternion.identity, size);
        }

        public Vector3 ScaleHandle(Vector3 previousScale, Vector3 position, Quaternion rotation, float size = 1) {
            return Handles.ScaleHandle(previousScale, position, rotation, size);
        }

        public Vector3 ScaleHandle(Vector3 previousScale, Vector3 position, string label, float size = 1) {
            Label(position, label);
            return ScaleHandle(previousScale, position, size);
        }

        public Vector3 ScaleHandle(Vector3 previousScale, Vector3 position, Quaternion rotation, string label,
            float size = 1) {
            Label(position, label);
            return ScaleHandle(previousScale, position, rotation, size);
        }
    }
}