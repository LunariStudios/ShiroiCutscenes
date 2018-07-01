using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Editor.Windows;
using Shiroi.Cutscenes.Futures;
using UnityEditor;
using UnityEngine;
using UnityUtilities;

namespace Shiroi.Cutscenes.Editor.Cutscenes {
    public partial class CutsceneEditor {
        private void DrawCutsceneHeader(GUISkin skin) {
            EditorGUILayout.BeginVertical(skin.box);
            DrawMainHeader(skin);
            DrawFutures(skin);
            EditorGUILayout.EndVertical();
        }

        private void DrawFutures(GUISkin skin) {
            EditorGUILayout.LabelField(
                ShiroiCutscenesEditorConstants.FuturesHeaderContent,
                skin.GetStyle(GUISkinProperties.HeaderLabel)
            );
            DrawFuturesList(skin);
        }

        private void DrawFuturesList(GUISkin skin) {
            var futures = Cutscene.Futures;
            if (futures.IsEmpty()) {
                EditorGUILayout.LabelField(
                    ShiroiCutscenesEditorConstants.EmptyFuturesContent
                );
            } else {
                foreach (var future in futures) {
                    DrawFuture(future, skin);
                }
            }
        }

        private void DrawFuture(ExpectedFuture future, GUISkin skin) { }

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
                ShiroiCutscenesEditorConstants.SettingsButtonContent,
                skin.GetStyle(GUISkinProperties.minibuttonright)
            );
        }

        private void DrawLeftCutsceneHeader(GUISkin skin) {
            EditorGUILayout.LabelField(
                ShiroiCutscenesEditorConstants.CutsceneEditorHeader,
                skin.GetStyle(GUISkinProperties.HeaderLabel)
            );
            DrawPlayerInfo(skin);
        }


        private void DrawPlayerInfo(GUISkin skin) {
            Player = (CutscenePlayer) EditorGUILayout.ObjectField(
                ShiroiCutscenesEditorConstants.EditorBoundPlayer,
                Player,
                typeof(CutscenePlayer),
                true
            );
            DrawPlayer(Player, skin);
        }

        private void DrawPlayer(CutscenePlayer player, GUISkin skin) {
            if (player != null) {
                CutscenePlayerEditor.DrawLayout(player, skin);
            } else {
                EditorGUILayout.LabelField(
                    ShiroiCutscenesEditorConstants.NoPlayerBoundContent,
                    skin.GetStyle(GUISkinProperties.CNStatusWarn)
                );
            }
        }
    }
}