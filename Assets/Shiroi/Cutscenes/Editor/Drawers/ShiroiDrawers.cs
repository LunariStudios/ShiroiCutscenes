using System;
using System.Linq;
using System.Reflection;
using Shiroi.Cutscenes.Futures;
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
            int tokenIndex, string name,
            object value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            EditorGUI.LabelField(rect,
                string.Format("Couldn't find drawer for field '{0}' of type '{1}'", name, valueType.Name));
        }
    }

    public class FutureDrawer<T> : TypeDrawer<FutureReference<T>> where T : Object {
        private readonly Type futureType = typeof(T);

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect,
            int tokenIndex, string name, FutureReference<T> value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            var futures = cutscene.GetFutures().ToList();
            futures.RemoveAll(future => !futureType.IsAssignableFrom(future.Type));
            futures.RemoveAll(future => future.Provider >= tokenIndex);
            var optionNames = futures.Select(future => future.Name).ToArray();
            var possibleOptions = futures.Select(future => future.Id).ToArray();

            value.Id = EditorGUI.IntPopup(rect, name, value.Id, optionNames, possibleOptions);
            setter(value);
        }
    }
}