using System;
using System.Collections.Generic;
using System.Linq;
using Lunari.Tsuki;
using Lunari.Tsuki.Editor;
using Shiroi.Cutscenes.Attributes;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Tokens;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Cutscenes {
    public partial class CutsceneEditor {
        public static readonly GUIContent CutsceneEditorHeader = new GUIContent(
            "Cutscene Metadata",
            "Information about the cutscene editor, bound player, futures or any kind of meta data about the current" +
            " cutscene is displayed here."
        );

        public static readonly GUIContent ClearCutsceneContent = new GUIContent(
            "Clear Cutscene",
            "Removes all the tokens from this cutscene"
        );

        public static readonly GUIContent EmptyFuturesContent = new GUIContent(
            "There are no futures on this cutscene",
            "This isn't bad by the way, we're just letting you know");

        public static readonly GUIContent TokenNameContent = new GUIContent(
            "Token Name",
            "The name of this token");

        public static readonly Color DeleteColor = new Color(0.8f, 0.24f, 0.24f);

        private void DrawTokens(GUISkin skin) {
            EditorGUILayout.BeginVertical(skin.box);
            DrawTokensHeader(skin);
            DrawTokensList(skin);
            EditorGUILayout.EndVertical();
        }

        private ushort currentTokenPage;
        private TypeSelectorPopupContent<Token> tokenPopUpContent;
        private List<UnityEditor.Editor> subEditors;


        public static void Swap<T>(IList<T> list, int indexA, int indexB) {
            var tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        private void DrawTokensList(GUISkin skin) {
            var toRemove = new HashSet<int>();
            bool shouldReloadEditors = false;
            for (var i = 0; i < subEditors.Count; i++) {
                var editor = subEditors[i];
                var token = Cutscene.GetToken(i);
                using (new EditorGUILayout.HorizontalScope(skin.box)) {
                    using (new EditorGUILayout.VerticalScope(ShiroiStyles.NoMargin)) {
                        var buttonProperties = new[] {
                            GUILayout.MaxWidth(50)
                        };
                        if (i > 0 && GUILayout.Button("Up", buttonProperties)) {
                            Swap(Cutscene.Tokens, i, i - 1);
                            shouldReloadEditors = true;
                        }

                        using (new GUIColorScope(DeleteColor)) {
                            if (GUILayout.Button("Delete", buttonProperties)) {
                                if (token != null && !EditorUtility.DisplayDialog(
                                        "You are about to delete " + token.name,
                                        "Are you sure about this?",
                                        "Yep", "Nope"
                                    )) {
                                    continue;
                                }

                                toRemove.Add(i);
                                DestroyImmediate(token, true);
                                shouldReloadEditors = true;
                            }
                        }

                        if (i < subEditors.Count - 1 && GUILayout.Button("Down", buttonProperties)) {
                            shouldReloadEditors = true;
                            Swap(Cutscene.Tokens, i, i + 1);
                        }
                    }

                    using (new EditorGUILayout.VerticalScope()) {
                        if (token == null) {
                            EditorGUILayout.LabelField("Null token, please delete");
                        } else {
                            using (new EditorGUILayout.HorizontalScope()) {
                                EditorGUILayout.LabelField($"#{i} - {token.name}",
                                    EditorStyles.boldLabel);
                                var hidden = (token.hideFlags & HideFlags.HideInHierarchy) == HideFlags.HideInHierarchy;
                                var img = GUIIcons.GetIcon(hidden
                                    ? GUIIcons.animationvisibilitytoggleoff
                                    : GUIIcons.animationvisibilitytoggleon);
                                if (GUILayout.Button(img, GUIStyle.none, GUILayout.Width(EditorGUIUtility.singleLineHeight))) {
                                    token.hideFlags ^= HideFlags.HideInHierarchy;
                                    AssetDatabase.SaveAssets();
                                }
                            }

                            token.name = EditorGUILayout.TextField(TokenNameContent, token.name);
                            editor.OnInspectorGUI();
                        }
                    }
                }
            }

            foreach (var i in toRemove) {
                Cutscene.Tokens.RemoveAt(i);
            }

            if (shouldReloadEditors) {
                ReloadSubEditors();
            }
        }

        private void DrawTokensHeader(GUISkin skin) {
            EditorGUILayout.LabelField(
                CutsceneEditorTokensHeader,
                skin.GetStyle(GUISkinProperties.HeaderLabel)
            );
            DrawTokensHeaderButtons(skin);
        }

        private Rect lastTokenRect;

        private void DrawTokensHeaderButtons(GUISkin skin) {
            EditorGUILayout.BeginHorizontal();
            var r = GUILayout.Button(
                AddTokenContent,
                GUISkinProperties.LargeButtonMid
            );
            if (Event.current.type == EventType.Repaint) {
                lastTokenRect = GUILayoutUtility.GetLastRect();
            }

            if (r) {
                PopupWindow.Show(lastTokenRect, tokenPopUpContent);
            }

            if (GUILayout.Button(
                ClearCutsceneContent,
                GUISkinProperties.LargeButtonMid
            )) {
                foreach (var token in Cutscene) {
                    DestroyImmediate(token, true);
                }

                Cutscene.Clear();
                ReloadSubEditors();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void TokensOnEnable() {
            tokenPopUpContent = new TypeSelectorPopupContent<Token>(() => lastTokenRect.width, type => {
                var t = (Token) Cutscene.AddToAssetFile(type);
                t.name = type.Name;
                t.hideFlags = HideFlags.HideInHierarchy;
                Cutscene.Add(t);
                ReloadSubEditors();
            });
            ReloadSubEditors();
        }

        private void ReloadSubEditors() {
            subEditors = new List<UnityEditor.Editor>();
            foreach (var token in Cutscene.Tokens) {
                subEditors.Add(CreateEditor(token));
            }
            AssetDatabase.SaveAssets();
        }
    }
}