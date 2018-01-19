using System;
using System.Linq;
using System.Reflection;
using Shiroi.Cutscenes.Futures;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public class FutureDrawer : TypeDrawer<FutureReference> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name,
            FutureReference value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            var futures = cutscene.GetFutures().ToList();
            var attribute = (FutureTypeAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(FutureTypeAttribute));
            if (attribute != null) {
                futures.RemoveAll(future => !attribute.Type.IsAssignableFrom(future.Type));
            }
            var optionNames = futures.Select(future => future.Name).ToArray();
            var possibleOptions = futures.Select(future => future.Id).ToArray();

            value.Id = EditorGUI.IntPopup(rect, name, value.Id, optionNames, possibleOptions);
            setter(value);
        }
    }
}