using Shiroi.Cutscenes.Editor.Util;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Windows {
    public class ContextWindow : PopupWindowContent {
        //Add token above
        //Add token below
        //Remove Tokens
        public const int TotalActions = 3;

        public static readonly Vector2 Size = new Vector2(200, TotalActions * ShiroiStyles.SingleLineHeight);
        public static readonly GUIContent NoTokenSelectedContent = new GUIContent("There is no token selected!");
        public static readonly GUIContent AddTokenAbove = new GUIContent("Add Token Above");

        public static readonly GUIContent RemoveTokenContent =
            new GUIContent("Remove Token", "Removes the currently selected token");

        public CutsceneEditor CurrentEditor { get; set; }

        public ContextWindow(CutsceneEditor currentEditor) {
            CurrentEditor = currentEditor;
        }

        public override Vector2 GetWindowSize() {
            return Size;
        }

        public override void OnGUI(Rect rect) {
            var list = CurrentEditor.TokenList;
            var hasSelected = list.HasSelected;
            var lastSelected = list.index;
            var labelRect = rect.GetLine(0);
            if (hasSelected) {
                GUI.Label(labelRect, "Editing token #" + lastSelected, ShiroiStyles.Header);
            } else {
                GUI.Label(labelRect, NoTokenSelectedContent, ShiroiStyles.Error);
            }

            GUI.enabled = hasSelected;
            if (GUI.Button(rect.GetLine(1), AddTokenAbove)) {
                var selector = CurrentEditor.SelectorWindow;
                PopupWindow.Show(rect, selector);
            }

            var initColor = GUI.backgroundColor;
            GUI.backgroundColor = ShiroiStyles.ErrorBackgroundColor;
            if (GUI.Button(rect.GetLine(2), RemoveTokenContent)) {
                CurrentEditor.Cutscene.RemoveToken(lastSelected);
                editorWindow.Close();
            }

            GUI.backgroundColor = initColor;
        }
    }
}