using System;
using System.Linq;
using System.Reflection;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Util;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public class NotFoundDrawer : TypeDrawer {
        public override int GetPriority() {
            return -1;
        }

        public override bool Supports(Type type) {
            return true;
        }

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect,
            int tokenIndex, GUIContent name, object value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            EditorGUI.LabelField(rect,
                string.Format("Couldn't find drawer for field '{0}' of type '{1}'", name, valueType.Name));
        }
    }

    public class ReferenceDrawer<T> : TypeDrawer<Reference<T>> where T : Object {
        public const float TypeWidth = 60;
        public const float LabelOffset = 15;

        //TODO: Make this less horrible
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect,
            int tokenIndex, GUIContent name, Reference<T> value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            if (value == null) {
                value = new Reference<T>(Reference.ReferenceType.Exposed, -1);
            }
            GUIContent label;
            var found = player == null ? null : value.Resolve(player);
            switch (value.Type) {
                case Reference.ReferenceType.Future:
                    label = name;
                    break;
                case Reference.ReferenceType.Exposed:
                    label = ExposedReferenceDrawer<T>.GetLabel(found, value.PropertyName, new GUIContent(name));
                    break;
                default:
                    return;
            }
            var labelWidth = GUIStyle.none.CalcSize(new GUIContent(label)).x;
            var r2 = rect.SubRect(TypeWidth, rect.height, labelWidth + LabelOffset);
            value.Type = (Reference.ReferenceType) EditorGUI.EnumPopup(r2, value.Type);
            switch (value.Type) {
                case Reference.ReferenceType.Exposed:
                    value.Id = DrawExposed(value, player, rect, name);
                    break;
                case Reference.ReferenceType.Future:
                    value.Id = DrawFuture(value, cutscene, rect, name, tokenIndex);
                    break;
            }
            setter(value);
        }

        private int DrawExposed(Reference<T> value, CutscenePlayer cutscene, Rect rect, GUIContent name) {
            return ExposedReferenceDrawer<T>.DrawExposed(cutscene, value, name, rect).exposedName.GetHashCode();
        }

        private int DrawFuture(Reference<T> value, Cutscene cutscene, Rect rect, GUIContent name, int tokenIndex) {
            return FutureDrawer<T>.DrawFuture(cutscene, tokenIndex, rect, name, value.Id);
        }
    }

    public class FutureDrawer<T> : TypeDrawer<FutureReference<T>> where T : Object {
        private static readonly Type FutureType = typeof(T);

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect,
            int tokenIndex, GUIContent name, FutureReference<T> value, Type valueType, FieldInfo fieldInfo,
            Setter setter) {
            if (value == null) {
                value = new FutureReference<T>(-1);
            }
            value.Id = DrawFuture(cutscene, tokenIndex, rect, name, value.Id);
            setter(value);
        }

        public static int DrawFuture(Cutscene cutscene, int tokenIndex, Rect rect, GUIContent name, int id) {
            var futures = cutscene.FutureManager.Futures.ToList();
            futures.RemoveAll(future => !FutureType.IsAssignableFrom(future.Type));
            futures.RemoveAll(future => future.Provider >= tokenIndex);
            var optionNames = futures.Select(future => new GUIContent(future.Name)).ToArray();
            var possibleOptions = futures.Select(future => future.Id).ToArray();

            return EditorGUI.IntPopup(rect, name, id, optionNames, possibleOptions);
        }
    }
}