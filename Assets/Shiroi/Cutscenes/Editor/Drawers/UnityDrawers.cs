using System;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public class Vector2Drawer : TypeDrawer<Vector2> {
        public override void Draw(Rect rect, string name, Vector2 value, Type valueType, Setter setter) {
            setter(EditorGUI.Vector2Field(rect, name, value));
        }
    }

    public class Vector3Drawer : TypeDrawer<Vector3> {
        public override void Draw(Rect rect, string name, Vector3 value, Type valueType, Setter setter) {
            setter(EditorGUI.Vector3Field(rect, name, value));
        }
    }

    public class Vector4Drawer : TypeDrawer<Vector4> {
        public override void Draw(Rect rect, string name, Vector4 value, Type valueType, Setter setter) {
            setter(EditorGUI.Vector4Field(rect, name, value));
        }
    }

    public class Vector2IntDrawer : TypeDrawer<Vector2Int> {
        public override void Draw(Rect rect, string name, Vector2Int value, Type valueType, Setter setter) {
            setter(EditorGUI.Vector2IntField(rect, name, value));
        }
    }

    public class Vector3IntDrawer : TypeDrawer<Vector3Int> {
        public override void Draw(Rect rect, string name, Vector3Int value, Type valueType, Setter setter) {
            setter(EditorGUI.Vector3IntField(rect, name, value));
        }
    }

    public class BoundsDrawer : TypeDrawer<Bounds> {
        public override void Draw(Rect rect, string name, Bounds value, Type valueType, Setter setter) {
            setter(EditorGUI.BoundsField(rect, name, value));
        }
    }

    public class BoundsIntDrawer : TypeDrawer<BoundsInt> {
        public override void Draw(Rect rect, string name, BoundsInt value, Type valueType, Setter setter) {
            setter(EditorGUI.BoundsIntField(rect, name, value));
        }
    }

    public class ColorDrawer : TypeDrawer<Color> {
        public override void Draw(Rect rect, string name, Color value, Type valueType, Setter setter) {
            setter(EditorGUI.ColorField(rect, name, value));
        }
    }

    public class AnimationCurveDrawer : TypeDrawer<AnimationCurve> {
        public override void Draw(Rect rect, string name, AnimationCurve value, Type valueType, Setter setter) {
            setter(EditorGUI.CurveField(rect, name, value));
        }
    }

    public class LayerMaskDrawer : TypeDrawer<LayerMask> {
        public override void Draw(Rect rect, string name, LayerMask value, Type valueType, Setter setter) {
            setter(EditorGUI.LayerField(rect, name, value));
        }
    }
}