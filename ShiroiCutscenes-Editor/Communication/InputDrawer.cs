using System.Collections.Generic;
using System.Linq;
using Lunari.Tsuki;
using Shiroi.Cutscenes.Communication;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEditor;
using UnityEngine;
using Input = Shiroi.Cutscenes.Communication.Input;

namespace Shiroi.Cutscenes.Editor.Communication {
    [CustomPropertyDrawer(typeof(Input), true)]
    public class InputDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent l) {
            var obj = property.serializedObject.targetObject as Token;
            if (obj == null) {
                EditorGUI.HelpBox(position, "Token not found! Inputs are required to exist within tokens!",
                    MessageType.Error);
                return;
            }

            var path = AssetDatabase.GetAssetPath(obj);
            var card = AssetDatabase.LoadAssetAtPath<Cutscene>(path);


            var variable = property.FindInstanceWithin<Input>();
            if (variable == null) {
                EditorGUI.HelpBox(position, $"Input not found! This should not happen! Please open an issue on GitHub!",
                    MessageType.Error);
                return;
            }

            var label = new GUIContent(l);

            label.text += $" (Input of: {variable.GetInputType().GetNiceName()})";
            label.image = ShiroiEditorUtil.GetIconFor(variable.GetInputType());
            if (card == null) {
                EditorGUI.LabelField(position, label);
                EditorGUI.HelpBox(position.AddXMin(EditorGUIUtility.labelWidth),
                    $"Cutscene not found! Tokens need to exist inside a Cutscene asset file! (Searched at path '{path}')",
                    MessageType.Error);
                return;
            }

            var minimumIndex = card.Tokens.IndexOf(obj);
            var allOutput = new List<Output>();
            for (var i = 0; i < card.Tokens.Count; i++) {
                if (minimumIndex > -1 && i > minimumIndex) {
                    continue;
                }

                if (!(card.Tokens[i] is IOutputContext src)) {
                    continue;
                }

                allOutput.AddRange(src.GetOutputs());
            }

            var compatibleFutures = allOutput.Where(future => variable.IsCompatibleWith(future)).ToList();

            var selectedIndex = 0;
            for (var i = 0; i < compatibleFutures.Count; i++) {
                var future = compatibleFutures[i];
                if (future.Name != variable.Name) {
                    continue;
                }

                selectedIndex = i;
                break;
            }


            if (compatibleFutures.Count <= 0) {
                var content = new GUIContent(
                    EditorGUIUtility.IconContent("console.erroricon")
                ) {
                    text = $"There are no compatible outputs! ({allOutput.Count} outputs searched.)"
                };
                EditorGUI.LabelField(position, label, content, EditorStyles.helpBox);
            } else {
                var names = from future in compatibleFutures select new GUIContent(future.Name);
                //position.xMin += position.height;
                var selected = EditorGUI.Popup(position, label, selectedIndex, names.ToArray());
                var selectedFuture = compatibleFutures[selected];
                variable.Name = selectedFuture.Name;

                var xOffset = EditorStyles.label.CalcSize(label).x;
                var colorRect = position;
                colorRect.xMin += xOffset;
                colorRect.size = OutputDrawer.ColorSquareSize;
                EditorGUI.DrawRect(colorRect.Padding(OutputDrawer.IconPadding), variable.GetColorFromName());
            }
        }
    }
}