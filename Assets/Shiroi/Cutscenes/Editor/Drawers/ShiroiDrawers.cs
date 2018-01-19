using System;
using System.Linq;
using Shiroi.Cutscenes.Futures;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public class FutureDrawer : TypeDrawer<FutureReference> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name,
            FutureReference value, Type valueType, Setter setter) {
            var possibleOptions = cutscene.GetFutures().Select(future => future.Id).ToArray();
            var currentlySelected = (int) cutscene.IndexOfFuture(value.Id);
            var optionNames = cutscene.GetFutureNames();
            value.Id = EditorGUI.IntPopup(rect, name, currentlySelected, optionNames, possibleOptions);
            setter(value);
        }
    }
}