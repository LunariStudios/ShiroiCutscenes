using System;
using Shiroi.Cutscenes.Editor.Util;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Cutscenes {
    public partial class CutsceneEditor {
        private void DrawTokens(GUISkin skin) {
            EditorGUILayout.BeginVertical(skin.box);
            DrawTokensHeader(skin);
            DrawTokensList(skin);
            EditorGUILayout.EndVertical();
        }

        private ushort currentTokenPage;
        private CutsceneTokenSelectorPopupContent tokenPopUpContent;


        private void DrawTokensList(GUISkin skin) { }

        private void DrawTokensHeader(GUISkin skin) {
            EditorGUILayout.LabelField(
                ShiroiCutscenesEditorConstants.CutsceneEditorTokensHeader,
                skin.GetStyle(GUISkinProperties.HeaderLabel)
            );
            DrawTokensHeaderButtons(skin);
        }

        private void DrawTokensHeaderButtons(GUISkin skin) {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(
                ShiroiCutscenesEditorConstants.AddTokenContent,
                GUISkinProperties.LargeButtonMid
            )) {
                if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Used) {
                    var r = GUILayoutUtility.GetLastRect();
                    PopupWindow.Show(r, tokenPopUpContent);
                    Debug.Log("Showing @ " + r);
                }
            }

            if (GUILayout.Button(
                ShiroiCutscenesEditorConstants.ClearCutsceneContent,
                GUISkinProperties.LargeButtonMid
            )) {
                Cutscene.Clear();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void TokensOnEnable() {
            tokenPopUpContent = new CutsceneTokenSelectorPopupContent(this);
        }
    }

    public class CutsceneTokenSelectorPopupContent : PopupWindowContent {
        // Label
        // Filter
        public const int BuiltInLines = 2;

        public const float WindowWidth = 300;

        public static Vector2 Size {
            get {
                return new Vector2(
                    WindowWidth,
                    (TokenLoader.KnownTokenTypes.Count + BuiltInLines) * ShiroiStyles.SingleLineHeight);
            }
        }

        public CutsceneTokenSelectorPopupContent(CutsceneEditor editor) {
            Editor = editor;
        }

        public CutsceneEditor Editor {
            get;
            private set;
        }

        public override Vector2 GetWindowSize() {
            return Size;
        }

        private string filter = string.Empty;

        public override void OnGUI(Rect rect) {
            EditorGUI.LabelField(rect.GetLine(0), "Select a token to add");
            EditorGUI.BeginChangeCheck();
            filter = SearchTextField(rect.GetLine(1), filter);
            if (EditorGUI.EndChangeCheck()) {
                editorWindow.Repaint();
                return;
            }

            var i = 0;
            foreach (var type in TokenLoader.KnownTokenTypes) {
                if (!string.IsNullOrEmpty(filter) &&
                    !type.Name.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase)) {
                    continue;
                }

                GUI.color = MappedToken.For(type).Color;
                if (GUI.Button(rect.GetLine((uint) (i + BuiltInLines)), type.Name)) {
                    Editor.AddToken(type);
                }

                i++;
            }
        }

        public static string SearchTextField(Rect rect, string searchString) {
            var changed = GUI.changed;
            var text = rect;
            text.xMax -= ShiroiStyles.SingleLineHeight;
            searchString = GUI.TextField(rect, searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
            var button = rect;
            button.xMin = text.xMax;
            if (GUI.Button(button, string.Empty, GUI.skin.FindStyle("ToolbarSeachCancelButton"))) {
                searchString = string.Empty;
            }

            GUI.changed = changed;
            return searchString;
        }
    }
}