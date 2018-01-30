using System;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Editor.Windows;
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

/*
Not forcing player selection for now
        private static readonly GUIContent NoSelectedPlayer =
            new GUIContent("You don't have any CutscenePlayer bound!");

        private static readonly GUIContent MultiplePlayersFound =
            new GUIContent("Found multiple CutscenePlayers within the current scene!");

        private static readonly GUIContent MultiplePlayersFoundWarning =
            new GUIContent("Please select one in order to assign ExposedReferences.");

        private static readonly GUIContent NoSelectedPlayerWarning =
            new GUIContent("You won't be able to assign ExposedReferences until you do so.");
*/
        public const float FuturesHeaderLines = 2.5F;

        public const float SpaceHeight = 5F;

        private static readonly GUIContent FuturesStats =
            new GUIContent("Futures Stats", "All of the info on your futures is listed below");

        private static readonly GUIContent PlayerHeader =
            new GUIContent("Player Settings", "All of the info on your Cutscene Player is listed below");

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

        public CutscenePlayer Player {
            get { return player; }
            private set {
                player = value;
                LastSelectedPlayer = value;
            }
        }

        public bool HasSelected {
            get { return LastSelected >= 0; }
        }

        public int LastSelected { get; private set; }

        public Cutscene Cutscene { get; private set; }

        private void OnEnable() {
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

        private void OnReorderCallback(ReorderableList list) {
            Cutscene.OnReorder(list.index, LastSelected);
        }

        private void OnRemoveCallback(ReorderableList list) {
            Cutscene.RemoveToken(list.index);
        }


        public override void OnInspectorGUI() {
            var totalTokens = Cutscene.Tokens.Count;
            /* Let's not annoy users with this for now
               if (Player == null) {
                  var found = FindObjectsOfType<CutscenePlayer>();
                  if (found.Length > 1) {
                      EditorGUILayout.LabelField(NoSelectedPlayer, ShiroiStyles.Error);
                  }
                  if (found.Length == 1) {
                      Player = found[0];
                  } else {
                      EditorGUILayout.LabelField(NoSelectedPlayer, ShiroiStyles.Error);
                      EditorGUILayout.LabelField(NoSelectedPlayerWarning, ShiroiStyles.Error);
                  }
              }*/
            DrawPlayerSettings();
            GUILayout.Space(SpaceHeight);
            //Reserve futures rect
            var totalFutures = Cutscene.TotalFutures;
            var hasFutures = totalFutures > 0;
            var futuresRect = default(Rect);
            if (hasFutures) {
                var futuresHeight = totalFutures * ShiroiStyles.IconSize +
                                    FuturesHeaderLines * EditorGUIUtility.singleLineHeight;
                futuresRect = GUILayoutUtility.GetRect(0, futuresHeight);
                GUILayout.Space(SpaceHeight);
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
            EditorGUILayout.BeginVertical(Player ? ShiroiStyles.DefaultBackground : ShiroiStyles.ErrorBackground);
            EditorGUILayout.LabelField(PlayerHeader, ShiroiStyles.Header);
            Player = (CutscenePlayer) EditorGUILayout.ObjectField(PlayerContent, Player, typeof(CutscenePlayer), true);
            if (Player) {
                ShiroiEditorUtil.DrawReferencesLayout(Player);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawTokens(int totalTokens) {
            EditorGUILayout.BeginVertical(ShiroiStyles.DefaultBackground);
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
            var futures = Cutscene.GetFutures();
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
                        msg = string.Format("{0} @ {3} (Owner: {1} @ #{2})", future.Name, token.GetType().Name, index,
                            future.Id);
                        var mappedToken = MappedToken.For(token);
                        color = LastSelected == index ? mappedToken.SelectedColor : mappedToken.Color;
                    }

                    var content = EditorGUIUtility.ObjectContent(null, future.Type);

                    var iconRect = futureRect.SubRect(iconSize, iconSize);
                    var msgRect = futureRect.SubRect(futureRect.width - iconSize, iconSize, iconSize);
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
                EditorGUILayout.LabelField(NoTokenContent, ShiroiStyles.Error);
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