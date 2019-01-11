using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Editor.Windows;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Cutscenes {
    public partial class CutsceneEditor {
        public static readonly GUIContent NoPlayerBoundContent = new GUIContent(
            "When a CutscenePlayer becomes bound, it's meta will be displayed here"
        );

        public static readonly GUIContent CutsceneEditorTokensHeader = new GUIContent(
            "Tokens",
            "All information about the tokens that structure this cutscenes"
        );

        public static readonly GUIContent SettingsButtonContent = new GUIContent(
            "Settings",
            "Open settings for the ShiroiCutscenes Editor"
        );

        private void DrawCutsceneHeader(GUISkin skin) {
            EditorGUILayout.BeginVertical(skin.box);
            DrawMainHeader(skin);
            EditorGUILayout.EndVertical();
        }

        private void DrawMainHeader(GUISkin skin) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            DrawLeftCutsceneHeader(skin);
            EditorGUILayout.EndVertical();
            DrawRightCutsceneHeader(skin);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawRightCutsceneHeader(GUISkin skin) {
            if (DrawSettingsButton(skin)) {
                ConfigWindow.ShowWindow();
            }
        }

        private bool DrawSettingsButton(GUISkin skin) {
            return GUILayout.Button(
                SettingsButtonContent,
                skin.GetStyle(GUISkinProperties.minibuttonright)
            );
        }

        private void DrawLeftCutsceneHeader(GUISkin skin) {
            EditorGUILayout.LabelField(
                CutsceneEditorHeader,
                skin.GetStyle(GUISkinProperties.HeaderLabel)
            );
        }
    }
}