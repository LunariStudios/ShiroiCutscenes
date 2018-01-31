using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Preview {
    public interface IScenePreviewable {
        void OnPreview(ISceneHandle handle, SceneView sceneView);
    }

    public interface ISceneHandle {
        void Label(Vector3 position, string label);
        void Label(Vector3 position, string label, GUIStyle style);

        Vector3 PositionHandle(Vector3 position);
        Vector3 PositionHandle(Vector3 position, string label);
        Vector3 PositionHandle(Vector3 position, Quaternion rotation);
        Vector3 PositionHandle(Vector3 position, Quaternion rotation, string label);

        Quaternion RotationHandle(Vector3 position, Quaternion previousRotation);
        Quaternion RotationHandle(Vector3 position, Quaternion previousRotation, string label);

        Vector3 ScaleHandle(Vector3 previousScale, Vector3 position, float size = 1);
        Vector3 ScaleHandle(Vector3 previousScale, Vector3 position, Quaternion rotation, float size = 1);
        Vector3 ScaleHandle(Vector3 previousScale, Vector3 position, string label, float size = 1);
        Vector3 ScaleHandle(Vector3 previousScale, Vector3 position, Quaternion rotation, string label, float size = 1);
    }
}