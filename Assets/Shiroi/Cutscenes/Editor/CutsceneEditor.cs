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
using UnityEditorInternal;
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
        private TokenList tokenList;

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

        public bool HasSelected {
            get {
                return LastSelected >= 0;
            }
        }

        public int LastSelected {
            get;
            private set;
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
            tokenList = new TokenList(this);
            /*tokenList = new ReorderableList(Cutscene.Tokens, typeof(IToken), true, false, false, true) {
                drawElementCallback = DrawToken,
                drawElementBackgroundCallback = DrawBackground,
                elementHeightCallback = CalculateHeight,
                onRemoveCallback = OnRemoveCallback,
                onReorderCallback = OnReorderCallback
            };*/
        }

        private void OnScene(SceneView sceneview) {
            var index = tokenList.index;
            if (index < 0 || index >= Cutscene.TotalTokens) {
                return;
            }
            var selectedToken = Cutscene[index] as IScenePreviewable;
            if (selectedToken != null) {
                selectedToken.OnPreview(EditorSceneHandle.Instance, sceneview);
            }
        }

        private void OnReorderCallback(ReorderableList list) {
            Cutscene.FutureManager.OnReorder(list.index, LastSelected);
        }

        private void OnRemoveCallback(ReorderableList list) {
            Cutscene.RemoveToken(list.index);
        }


        //Errors


        public override void OnInspectorGUI() {
            var checkErrors = (bool) Configs.CheckErrors;
            if (checkErrors) {
                ErrorManager.Clear();
            }
            var totalTokens = Cutscene.Tokens.Count;
            DrawPlayerSettings();
            GUILayout.Space(ShiroiStyles.SpaceHeight);
            //Reserve futures rect
            var totalFutures = Cutscene.FutureManager.TotalFutures;
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
            if (Event.current.type == EventType.ContextClick) {
                var rect = new Rect(Event.current.mousePosition, ContextWindow.Size);
                PopupWindow.Show(rect, ContextWindow);
            }
        }

        private void DrawPlayerSettings() {
            EditorGUILayout.BeginVertical(Player ? ShiroiStyles.DefaultBackground : ShiroiStyles.Error);
            EditorGUILayout.LabelField(Player ? ShiroiStyles.PlayerHeader : ShiroiStyles.PlayerHeaderError,
                ShiroiStyles.Header);
            Player = (CutscenePlayer) EditorGUILayout.ObjectField(ShiroiStyles.PlayerContent, Player,
                typeof(CutscenePlayer), true);
            if (Player) {
                ShiroiEditorUtil.DrawReferencesLayout(Player);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawTokens(int totalTokens) {
            var empty = totalTokens == 0;
            EditorGUILayout.BeginVertical(empty ? ShiroiStyles.Error : ShiroiStyles.DefaultBackground);
            DrawCutsceneHeader(totalTokens);
            if (totalTokens > 0) {
                hasAnyFocused = false;
                tokenList.Draw();
                if (!hasAnyFocused) {
                    LastSelected = -1;
                }
            }
            EditorGUILayout.EndVertical();
        }


        private void DrawFutures(Rect rect) {
            GUI.Box(rect, GUIContent.none, ShiroiStyles.DefaultBackground);
            var futures = Cutscene.FutureManager.GetFutures();
            var totalFutures = futures.Count;
            EditorGUI.LabelField(rect.GetLine(0), ShiroiStyles.FuturesStats, ShiroiStyles.Header);
            var labelRect = rect.GetLine(1);
            if (totalFutures == 0) {
                EditorGUI.LabelField(labelRect, "No futures registered.");
            } else {
                futures.Sort();
                EditorGUI.LabelField(labelRect, totalFutures + " futures found!",
                    ShiroiStyles.Header);
                const int iconSize = ShiroiStyles.IconSize;
                var yOffset = EditorGUIUtility.singleLineHeight * ShiroiStyles.FuturesHeaderLines;
                var initColor = GUI.backgroundColor;
                var totalTokens = Cutscene.TotalTokens;
                for (var i = 0; i < futures.Count; i++) {
                    var future = futures[i];
                    var index = future.Provider;
                    string msg;
                    Color color;
                    var futureRect = rect.GetLine((uint) i, collumHeight: iconSize,
                        yOffset: yOffset);
                    if (index < 0) {
                        msg = string.Format("{0} #{1} (Error! Owner not found)", future.Name, index);
                        EditorGUI.LabelField(futureRect, msg);
                        color = ShiroiStyles.ErrorBackgroundColor;
                    } else {
                        var token = Cutscene[index];
                        var tokenName = MappedToken.For(token).Label;
                        msg = string.Format("{0} @ {3} (Owner: {1} @ #{2})", future.Name, tokenName, index,
                            future.Id);
                        var mappedToken = MappedToken.For(token);
                        color = LastSelected == index ? mappedToken.SelectedColor : mappedToken.Color;
                    }

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

        private void DrawCutsceneHeader(int totalLines) {
            EditorGUILayout.LabelField("Tokens", ShiroiStyles.Header);
            var isEmpty = totalLines == 0;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(ShiroiStyles.AddTokenContent)) {
                var rect = new Rect(Event.current.mousePosition, TokenSelectorWindow.Size);
                PopupWindow.Show(rect, SelectorWindow);
            }
            if (!isEmpty) {
                if (GUILayout.Button(ShiroiStyles.ClearCutscene)) {
                    Cutscene.Clear();
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

        private void DrawBackground(Rect rect, int index, bool isactive, bool isfocused) {
            if (isfocused) {
                hasAnyFocused = true;
                LastSelected = index;
            }
            if (index == -1) {
                return;
            }

            var m = MappedToken.For(Cutscene[index]);
            var initColor = GUI.backgroundColor;
            GUI.backgroundColor = isfocused ? m.SelectedColor : m.Color;
            GUI.Box(rect, GUIContent.none);
            GUI.backgroundColor = initColor;
        }

        private float CalculateHeight(int index) {
            var token = Cutscene[index];
            return MappedToken.For(token).Height;
        }


        private void DrawToken(Rect rect, int index, bool isactive, bool isfocused) {
            var token = Cutscene[index];
            var mappedToken = MappedToken.For(token);
            bool changed;
            mappedToken.DrawFields(this, rect, index, token, Cutscene, Player, out changed);
            if (!changed) {
                return;
            }
            EditorUtility.SetDirty(Cutscene);
            var l = token as ITokenChangedListener;
            if (l != null) {
                l.OnChanged(Cutscene);
            }
        }

        public void AddToken(Type type) {
            var instance = (IToken) Activator.CreateInstance(type);
            if (Cutscene.IsEmpty || LastSelected < 0) {
                Cutscene.AddToken(instance);
            } else {
                Cutscene.AddToken(LastSelected, instance);
            }
            EditorUtility.SetDirty(this);
            SetCutsceneDirty();
            //tokenList.GrabKeyboardFocus();
            tokenList.index = Cutscene.Tokens.Count - 1;
        }

        private void SetCutsceneDirty() {
            EditorUtility.SetDirty(Cutscene);
        }
    }
}