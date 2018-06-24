using System;
using System.Collections.Generic;
using Shiroi.Cutscenes.Editor.Config;
using Shiroi.Cutscenes.Editor.Errors;
using Shiroi.Cutscenes.Editor.Preview;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Editor.Windows;
using Shiroi.Cutscenes.Preview;
using Shiroi.Cutscenes.Tokens;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shiroi.Cutscenes.Editor {
    [CustomEditor(typeof(Cutscene))]
    public class CutsceneEditor : UnityEditor.Editor {
        public static CutscenePlayer LastSelectedPlayer;
        private static readonly List<CutsceneEditor> editors = new List<CutsceneEditor>();


        public static IEnumerable<CutsceneEditor> Editors {
            get {
                return editors;
            }
        }

        private static int currentKaomoji;

        private static string GetKaomoji() {
            return ShiroiStyles.Kaomojis[currentKaomoji];
        }


        //Instance Fields
        public TokenList TokenList;

        private bool hasAnyFocused;

        [SerializeField]
        private CutscenePlayer player;

        public ContextWindow ContextWindow;
        public TokenSelectorWindow SelectorWindow;
        public readonly ErrorManager ErrorManager = new ErrorManager();

        public CutscenePlayer Player {
            get {
                return player;
            }
            private set {
                player = value;
                LastSelectedPlayer = value;
            }
        }

        public Cutscene Cutscene {
            get;
            private set;
        }

        private void OnDisable() {
            SceneView.onSceneGUIDelegate -= OnScene;
            editors.Remove(this);
        }

        private void OnEnable() {
            editors.Add(this);
            SceneView.onSceneGUIDelegate += OnScene;
            Player = LastSelectedPlayer;
            ContextWindow = new ContextWindow(this);
            SelectorWindow = new TokenSelectorWindow(this);
            Cutscene = (Cutscene) target;
            TokenList = new TokenList(this);
        }

        private void OnScene(SceneView sceneview) {
            var index = TokenList.index;
            if (index < 0 || index >= Cutscene.Count) {
                return;
            }

            var selectedToken = Cutscene[index] as IScenePreviewable;
            if (selectedToken != null) {
                selectedToken.OnPreview(EditorSceneHandle.Instance);
            }
        }


        public override void OnInspectorGUI() {
            var checkErrors = (bool) Configs.CheckErrors;
            if (checkErrors) {
                ErrorManager.Clear();
            }

            var totalTokens = Cutscene.Count;
            DrawPlayerSettings();
            GUILayout.Space(ShiroiStyles.SpaceHeight);
            //Reserve futures rect
            var totalFutures = Cutscene.TotalFutures;
            var hasFutures = totalFutures > 0;
            var futuresRect = default(Rect);
            if (hasFutures) {
                var futuresHeight = totalFutures * ShiroiStyles.IconSize +
                                    ShiroiStyles.FuturesHeaderLines * EditorGUIUtility.singleLineHeight;
                futuresRect = GUILayoutUtility.GetRect(0, futuresHeight);
                GUILayout.Space(ShiroiStyles.SpaceHeight);
            }

            if (checkErrors) {
                ErrorManager.DrawErrors(this);
            }

            DrawTokens(totalTokens);
            if (hasFutures) {
                DrawFutures(futuresRect);
            }

            var e = Event.current;
            if (e.type == EventType.ContextClick || e.isMouse && e.button == 1) {
                var rect = new Rect(Event.current.mousePosition, ContextWindow.Size);
                PopupWindow.Show(rect, ContextWindow);
            }
        }

        private void DrawPlayerSettings() {
            EditorGUILayout.BeginVertical(Player ? ShiroiStyles.DefaultBackground : ShiroiStyles.Error);
            EditorGUILayout.LabelField(
                Player ? ShiroiStyles.PlayerHeader : ShiroiStyles.PlayerHeaderError,
                ShiroiStyles.Header);
            Player = (CutscenePlayer) EditorGUILayout.ObjectField(
                ShiroiStyles.PlayerContent,
                Player,
                typeof(CutscenePlayer),
                true);
            if (Player) {
                ShiroiEditorUtil.DrawReferencesLayout(Player);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawTokens(int totalTokens) {
            var empty = totalTokens == 0;
            EditorGUILayout.BeginVertical(empty ? ShiroiStyles.Error : ShiroiStyles.DefaultBackground);
            bool cleared;
            DrawCutsceneHeader(totalTokens, out cleared);
            if (!cleared && totalTokens > 0) {
                hasAnyFocused = false;
                TokenList.Draw();
            }

            EditorGUILayout.EndVertical();
        }


        private void DrawFutures(Rect rect) {
            GUI.Box(rect, GUIContent.none, ShiroiStyles.DefaultBackground);
            var futures = Cutscene.Futures;
            var totalFutures = futures.Count;
            EditorGUI.LabelField(rect.GetLine(0), ShiroiStyles.FuturesStats, ShiroiStyles.Header);
            var labelRect = rect.GetLine(1);
            if (totalFutures == 0) {
                EditorGUI.LabelField(labelRect, "No futures registered.");
            } else {
                EditorGUI.LabelField(
                    labelRect,
                    totalFutures + " futures found!",
                    ShiroiStyles.Header);
                const int iconSize = ShiroiStyles.IconSize;
                var yOffset = EditorGUIUtility.singleLineHeight * ShiroiStyles.FuturesHeaderLines;
                var initColor = GUI.backgroundColor;
                var totalTokens = Cutscene.Count;
                for (var i = 0; i < futures.Count; i++) {
                    var future = futures[i];
                    var token = future.Provider;
                    var index = Cutscene.IndexOf(token);
                    var futureRect = rect.GetLine(
                        (uint) i,
                        collumHeight: iconSize,
                        yOffset: yOffset);

                    var tokenName = MappedToken.For(token).Label;
                    var msg = string.Format(
                        "{0} @ {3} (Owner: {1} @ #{2})",
                        future.Name,
                        tokenName,
                        index,
                        future.Id);
                    var mappedToken = MappedToken.For(token);
                    var color = TokenList.index == index ? mappedToken.SelectedColor : mappedToken.Color;
                    var content = EditorGUIUtility.ObjectContent(null, future.Type);

                    Rect iconRect;
                    Rect msgRect;
                    futureRect.Split(iconSize, out iconRect, out msgRect);
                    content.text = null;
                    GUI.backgroundColor = color;
                    GUI.Box(futureRect, GUIContent.none);
                    GUI.Box(iconRect, content);
                    EditorGUI.LabelField(msgRect, msg);
                }

                GUI.backgroundColor = initColor;
            }
        }

        private void DrawCutsceneHeader(int totalLines, out bool cleared) {
            EditorGUILayout.LabelField("Tokens", ShiroiStyles.Header);
            var isEmpty = totalLines == 0;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(ShiroiStyles.AddTokenContent)) {
                var rect = new Rect(Event.current.mousePosition, TokenSelectorWindow.Size);
                PopupWindow.Show(rect, SelectorWindow);
            }

            cleared = false;
            if (!isEmpty) {
                if (GUILayout.Button(ShiroiStyles.ClearCutscene)) {
                    cleared = true;
                    Clear(Cutscene);
                }
            }

            EditorGUILayout.EndHorizontal();
            //No token in cutscene
            if (isEmpty) {
                EditorGUILayout.LabelField(ShiroiStyles.NoTokenContent, ShiroiStyles.HeaderCenter);
                GUI.enabled = false;
                EditorGUILayout.LabelField(GetKaomoji(), ShiroiStyles.Kaomoji, GUILayout.ExpandHeight(true));
            } else {
                //Reload kaomojis
                currentKaomoji = Random.Range(0, ShiroiStyles.Kaomojis.Length - 1);
            }
        }

        private void Clear(Cutscene cutscene) {
            foreach (var token in cutscene) {
                DestroyImmediate(token, true);
            }

            AssetDatabase.SaveAssets();
            Cutscene.Clear();
        }

        public void AddToken(Type type) {
            var instance = (Token) CreateInstance(type);
            instance.name = type.Name;
            if (Cutscene.IsEmpty || TokenList.index < 0) {
                Cutscene.Add(instance);
            } else {
                Cutscene.Add(TokenList.index, instance);
            }

            AssetDatabase.AddObjectToAsset(instance, Cutscene);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(this);
            SetCutsceneDirty();
            //tokenList.GrabKeyboardFocus();
            TokenList.index = Cutscene.Count - 1;
        }

        private void SetCutsceneDirty() {
            EditorUtility.SetDirty(Cutscene);
        }

        public void RemoveToken(int lastSelected) {
            var t = Cutscene[lastSelected];
            Cutscene.RemoveToken(lastSelected);
            DestroyImmediate(t, true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}