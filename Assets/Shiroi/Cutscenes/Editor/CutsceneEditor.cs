using System;
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
        public static readonly Vector2 TokenWindowSize = new Vector2(150, 500);
        public static CutscenePlayer LastSelectedPlayer;

        private static int currentKaomoji;


        private static readonly GUIContent ClearCutscene =
            new GUIContent("Clear cutscene", "Removes all tokens");

        public const float FuturesHeaderLines = 2.5F;


        private static readonly GUIContent FuturesStats =
            new GUIContent("Futures Stats", "All of the info on your futures is listed below");

        private static readonly GUIContent PlayerHeader =
            new GUIContent("Player Settings", "All of the info on your Cutscene Player is listed below");

        private static readonly GUIContent PlayerHeaderError = new GUIContent(PlayerHeader);

        private static readonly GUIContent PlayerContent =
            new GUIContent("Bound player", "The player that will store the references to the cutscene's scene objects");

        private static readonly GUIContent AddTokenContent =
            new GUIContent("Choose your flavour", "Adds a token to the cutscene");

        private static readonly GUIContent NoTokenContent =
            new GUIContent("There aren't any tokens defined!", "There are no tokens in the cutscene for you to edit.");

        private static readonly string[] Kaomojis = {
            ":(",
            "(^-^*)",
            "(;-;)",
            "(o^^)o",
            "(>_<)",
            "\\(^Д^)/",
            "(≥o≤)",
            "(·.·)",
            "╯°□°）╯︵ ┻━┻"
        };

        private static string GetKaomoji() {
            return Kaomojis[currentKaomoji];
        }


        //Instance Fields
        private ReorderableList tokenList;

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
        }

        private void OnEnable() {
            PlayerHeaderError.image = ShiroiStyles.ErrorIcon;
            SceneView.onSceneGUIDelegate += OnScene;
            Player = LastSelectedPlayer;
            ContextWindow = new ContextWindow(this);
            SelectorWindow = new TokenSelectorWindow(this);
            Cutscene = (Cutscene) target;
            tokenList = new ReorderableList(Cutscene.Tokens, typeof(IToken), true, false, false, true) {
                drawElementCallback = DrawToken,
                drawElementBackgroundCallback = DrawBackground,
                elementHeightCallback = CalculateHeight,
                onRemoveCallback = OnRemoveCallback,
                onReorderCallback = OnReorderCallback
            };
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
            ErrorManager.Clear();
            var totalTokens = Cutscene.Tokens.Count;
            DrawPlayerSettings();
            GUILayout.Space(ShiroiStyles.SpaceHeight);
            //Reserve futures rect
            var totalFutures = Cutscene.FutureManager.TotalFutures;
            var hasFutures = totalFutures > 0;
            var futuresRect = default(Rect);
            if (hasFutures) {
                var futuresHeight = totalFutures * ShiroiStyles.IconSize +
                                    FuturesHeaderLines * EditorGUIUtility.singleLineHeight;
                futuresRect = GUILayoutUtility.GetRect(0, futuresHeight);
                GUILayout.Space(ShiroiStyles.SpaceHeight);
            }
            ErrorManager.DrawErrors(this);
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
            EditorGUILayout.LabelField(Player ? PlayerHeader : PlayerHeaderError, ShiroiStyles.Header);
            Player = (CutscenePlayer) EditorGUILayout.ObjectField(PlayerContent, Player, typeof(CutscenePlayer), true);
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
                tokenList.DoLayoutList();
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
            EditorGUI.LabelField(rect.GetLine(0), FuturesStats, ShiroiStyles.Header);
            var labelRect = rect.GetLine(1);
            if (totalFutures == 0) {
                EditorGUI.LabelField(labelRect, "No futures registered.");
            } else {
                futures.Sort();
                EditorGUI.LabelField(labelRect, totalFutures + " futures found!",
                    ShiroiStyles.Header);
                const int iconSize = ShiroiStyles.IconSize;
                var yOffset = EditorGUIUtility.singleLineHeight * FuturesHeaderLines;
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
            if (GUILayout.Button(AddTokenContent)) {
                var rect = new Rect(Event.current.mousePosition, TokenSelectorWindow.Size);
                PopupWindow.Show(rect, SelectorWindow);
            }
            if (!isEmpty) {
                if (GUILayout.Button(ClearCutscene)) {
                    Cutscene.Clear();
                }
            }

            EditorGUILayout.EndHorizontal();
            //No token in cutscene
            if (isEmpty) {
                EditorGUILayout.LabelField(NoTokenContent, ShiroiStyles.HeaderCenter);
                GUI.enabled = false;
                EditorGUILayout.LabelField(GetKaomoji(), ShiroiStyles.Kaomoji, GUILayout.ExpandHeight(true));
            } else {
                //Reload kaomojis
                currentKaomoji = Random.Range(0, Kaomojis.Length - 1);
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
            var labelRect = GetRect(rect, 0);
            var mappedToken = MappedToken.For(token);
            EditorGUI.LabelField(labelRect, string.Format("#{0} - {1}", index, mappedToken.Label), ShiroiStyles.Bold);
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


        public static Rect GetRect(Rect rect, int index) {
            var x = rect.x;
            var y = rect.y;
            return new Rect(x, y + index * EditorGUIUtility.singleLineHeight, rect.width,
                EditorGUIUtility.singleLineHeight);
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
            tokenList.GrabKeyboardFocus();
            tokenList.index = Cutscene.Tokens.Count - 1;
        }

        private void SetCutsceneDirty() {
            EditorUtility.SetDirty(Cutscene);
        }
    }
}