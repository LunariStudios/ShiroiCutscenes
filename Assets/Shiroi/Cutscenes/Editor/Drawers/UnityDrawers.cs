using System;
using System.Reflection;
using NUnit.Framework.Internal;
using Shiroi.Cutscenes.Util;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public class ExposedReferenceDrawer<T> : TypeDrawer<ExposedReference<T>> where T : Object {
        private readonly Type referenceType = typeof(T);

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, ExposedReference<T> value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            GUI.enabled = player != null;
            var chosen = EditorGUI.ObjectField(rect, name, value.Resolve(player), referenceType, true);
            GUI.enabled = true;
            if (chosen != null) {
                value.exposedName = chosen.GetInstanceID().ToString();
            }
            if (player != null) {
                player.SetReferenceValue(value.exposedName, chosen);
            }

            setter(value);
        }
    }

    public class ObjectDrawer : TypeDrawer<Object> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, Object value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.ObjectField(rect, name, value, valueType, false));
        }
    }

    public class QuaternionDrawer : TypeDrawer<Quaternion> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, Quaternion value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(Quaternion.Euler(EditorGUI.Vector3Field(rect, name, value.eulerAngles)));
        }
    }

    public class Vector2Drawer : TypeDrawer<Vector2> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, Vector2 value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.Vector2Field(rect, name, value));
        }
    }

    public class Vector3Drawer : TypeDrawer<Vector3> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, Vector3 value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.Vector3Field(rect, name, value));
        }
    }

    public class Vector4Drawer : TypeDrawer<Vector4> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, Vector4 value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.Vector4Field(rect, name, value));
        }
    }

    public class Vector2IntDrawer : TypeDrawer<Vector2Int> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, Vector2Int value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.Vector2IntField(rect, name, value));
        }
    }

    public class Vector3IntDrawer : TypeDrawer<Vector3Int> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, Vector3Int value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.Vector3IntField(rect, name, value));
        }
    }

    public class BoundsDrawer : TypeDrawer<Bounds> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, Bounds value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.BoundsField(rect, name, value));
        }
    }

    public class BoundsIntDrawer : TypeDrawer<BoundsInt> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, BoundsInt value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.BoundsIntField(rect, name, value));
        }
    }

    public class ColorDrawer : TypeDrawer<Color> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, Color value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.ColorField(rect, name, value));
        }
    }

    public class AnimationCurveDrawer : TypeDrawer<AnimationCurve> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, AnimationCurve value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter(EditorGUI.CurveField(rect, name, value));
        }
    }

    public class LayerMaskDrawer : TypeDrawer<LayerMask> {
        public override byte GetPriority() {
            //Prefer over int
            return 1;
        }

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, string name, LayerMask value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            setter((LayerMask) EditorGUI.LayerField(rect, name, value));
        }
    }
}