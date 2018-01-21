using System;
using System.Linq;
using System.Reflection;
using Shiroi.Cutscenes.Futures;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public class NotFoundDrawer : TypeDrawer {
        public override int GetPriority() {
            return -1;
        }

        public override bool Supports(Type type) {
            return true;
        }

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect,
            int tokenIndex, string name,
            object value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            EditorGUI.LabelField(rect,
                string.Format("Couldn't find drawer for field '{0}' of type '{1}'", name, valueType.Name));
        }
    }

    public class FutureDrawer : TypeDrawer<FutureReference> {
        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect,
            int tokenIndex, string name, FutureReference value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            var futures = cutscene.GetFutures().ToList();
            var attribute = (FutureTypeAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(FutureTypeAttribute));
            if (attribute != null) {
                futures.RemoveAll(future => !attribute.Type.IsAssignableFrom(future.Type));
            }
            futures.RemoveAll(future => future.Provider >= tokenIndex);
            var optionNames = futures.Select(future => future.Name).ToArray();
            var possibleOptions = futures.Select(future => future.Id).ToArray();

            value.Id = EditorGUI.IntPopup(rect, name, value.Id, optionNames, possibleOptions);
            setter(value);
        }
    }
}