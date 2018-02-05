using System;
using System.Reflection;
using NUnit.Framework.Internal;
using Shiroi.Cutscenes.Util;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public class ExposedReferenceDrawer<T> : TypeDrawer<ExposedReference<T>> where T : Object {
        private static readonly Type ReferenceType = typeof(T);

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, ExposedReference<T> value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(DrawExposed(player, value, name, rect));
        }

        public static GUIContent GetLabel(Object found, PropertyName value, GUIContent name) {
            var msg = found == null
                ? "null"
                : string.Format("{0}:{1}", found.GetInstanceID(), value.GetHashCode());
            name.text = string.Format("{0} ({1})", name.text, msg);
            return name;
        }

        public static ExposedReference<T> DrawExposed(CutscenePlayer player, ExposedReference<T> value, GUIContent name,
            Rect rect) {
            GUI.enabled = player != null;
            var found = value.Resolve(player);
            var label = GetLabel(found, value.exposedName, name);
            var chosen = EditorGUI.ObjectField(rect, label, found, ReferenceType, true);
            GUI.enabled = true;

            if (player == null || chosen == found) {
                return value;
            }
            //Remove old
            player.SetReferenceValue(value.exposedName, null);
            //If there is a new one, add it
            if (chosen != null) {
                var newId = chosen.GetInstanceID().ToString();
                value.exposedName = newId;
                player.SetReferenceValue(newId, chosen);
            }
            return value;
        }
    }

    public class ObjectDrawer : TypeDrawer<Object> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, Object value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.ObjectField(rect, name, value, valueType, false));
        }
    }

    public class QuaternionDrawer : TypeDrawer<Quaternion> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, Quaternion value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(Quaternion.Euler(EditorGUI.Vector3Field(rect, name, value.eulerAngles)));
        }
    }

    public class Vector2Drawer : TypeDrawer<Vector2> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, Vector2 value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.Vector2Field(rect, name, value));
        }
    }

    public class Vector3Drawer : TypeDrawer<Vector3> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, Vector3 value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.Vector3Field(rect, name, value));
        }
    }

    public class Vector4Drawer : TypeDrawer<Vector4> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, Vector4 value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.Vector4Field(rect, name, value));
        }
    }

    public class Vector2IntDrawer : TypeDrawer<Vector2Int> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, Vector2Int value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.Vector2IntField(rect, name, value));
        }
    }

    public class Vector3IntDrawer : TypeDrawer<Vector3Int> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, Vector3Int value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.Vector3IntField(rect, name, value));
        }
    }

    public class BoundsDrawer : TypeDrawer<Bounds> {
        public const uint BoundsFieldSize = 3;

        public override uint GetTotalLines() {
            return BoundsFieldSize;
        }

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, Bounds value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.BoundsField(rect, name, value));
        }
    }

    public class BoundsIntDrawer : TypeDrawer<BoundsInt> {
        public override uint GetTotalLines() {
            return BoundsDrawer.BoundsFieldSize;
        }

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, BoundsInt value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.BoundsIntField(rect, name, value));
        }
    }

    public class ColorDrawer : TypeDrawer<Color> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, Color value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.ColorField(rect, name, value));
        }
    }

    public class AnimationCurveDrawer : TypeDrawer<AnimationCurve> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, AnimationCurve value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.CurveField(rect, name, value));
        }
    }

    public class RectDrawer : TypeDrawer<Rect> {
        public const uint RectFieldSize = 2;

        public override uint GetTotalLines() {
            return RectFieldSize;
        }

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, Rect value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.RectField(rect, name, value));
        }
    }

    public class RectIntDrawer : TypeDrawer<RectInt> {
        public override uint GetTotalLines() {
            return RectDrawer.RectFieldSize;
        }

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, RectInt value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.RectIntField(rect, name, value));
        }
    }

    public class LayerMaskDrawer : TypeDrawer<LayerMask> {
        public override int GetPriority() {
            //Prefer over int
            return 1;
        }

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, LayerMask value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter((LayerMask) EditorGUI.LayerField(rect, name, value));
        }
    }
}