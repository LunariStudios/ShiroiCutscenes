using System;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Editor.Util;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shiroi.Cutscenes.Editor {
    [CustomEditor(typeof(Cutscene))]
    public class CutsceneEditor : UnityEditor.Editor {
        public static readonly Vector2 TokenWindowSize = new Vector2(150, 500);


        private static int currentKaomoji;
        public TokenSelectorWindow TokenSelector;
        public CutscenePlayer Player { get; private set; }

        private static readonly GUIContent ClearCutscene =
            new GUIContent("Clear cutscene", "Removes all tokens");

        private static readonly GUIContent NoSelectedPlayer =
            new GUIContent("You don't have any CutscenePlayer bound!");

        private static readonly GUIContent MultiplePlayersFound =
            new GUIContent("Found multiple CutscenePlayers within the current scene!");

        private static readonly GUIContent MultiplePlayersFoundWarning =
            new GUIContent("Please select one in order to assign ExposedReferences.");

        private static readonly GUIContent NoSelectedPlayerWarning =
            new GUIContent("You won't be able to assign ExposedReferences until you do so.");

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

        private static void LoadStyles() { }

        private ReorderableList tokenList;

        private Cutscene cutscene;
        private int lastSelected;

        private void OnEnable() {
            LoadStyles();
            TokenSelector = new TokenSelectorWindow(this);
            cutscene = (Cutscene) target;
            tokenList = new ReorderableList(cutscene.Tokens, typeof(IToken), true, true, false, true) {
                drawHeaderCallback = DrawTokensHeader,
                drawElementCallback = DrawToken,
                drawElementBackgroundCallback = DrawBackground,
                elementHeightCallback = CalculateHeight,
                onRemoveCallback = OnRemoveCallback,
                onReorderCallback = OnReorderCallback
            };
        }

        private void OnReorderCallback(ReorderableList list) {
            cutscene.OnReorder(list, list.index, lastSelected);
        }

        private void OnRemoveCallback(ReorderableList list) {
            cutscene.RemoveToken(list.index);
        }


        public override void OnInspectorGUI() {
            var totalTokens = cutscene.Tokens.Count;
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
            }
            Player = (CutscenePlayer) EditorGUILayout.ObjectField(PlayerContent, Player, typeof(CutscenePlayer), true);
            DrawCutsceneHeader(totalTokens);
            //Reserve futures rect
            var totalFutures = cutscene.TotalFutures;
            var hasFutures = totalFutures > 0;
            Rect futuresRect = default(Rect);
            if (hasFutures) {
                var futuresHeight = totalFutures * ShiroiStyles.IconSize + 2 * EditorGUIUtility.singleLineHeight;
                futuresRect = GUILayoutUtility.GetRect(0, futuresHeight);
            }
            //
            if (totalTokens <= 0) {
                return;
            }
            tokenList.DoLayoutList();
            lastSelected = tokenList.index;
            //
            if (hasFutures) {
                DrawFutures(futuresRect);
            }
        }

        private void DrawFutures(Rect rect) {
            var futures = cutscene.GetFutures();
            var totalFutures = futures.Count;
            var labelRect = rect.GetLine(0);
            if (totalFutures == 0) {
                EditorGUI.LabelField(labelRect, "No futures registered.");
            } else {
                futures.Sort();
                EditorGUI.LabelField(labelRect, totalFutures + " futures found!", ShiroiStyles.Header);
                var iconSize = ShiroiStyles.IconSize;
                for (var i = 0; i < futures.Count; i++) {
                    var future = futures[i];
                    var index = future.Provider;
                    var token = cutscene[index];
                    var msg = string.Format("{0} @ {3} (Owner: {1} @ {2})", future.Name, token.GetType().Name, index,
                        future.Id);
                    var mappedToken = MappedToken.For(token);
                    var style = tokenList.index == index ? mappedToken.SelectedStyle : mappedToken.Style;
                    var content = EditorGUIUtility.ObjectContent(null, future.Type);
                    var futureRect = rect.GetLine((uint) (i + 1), collumHeight: iconSize);
                    var iconRect = futureRect.SubRect(iconSize, iconSize);
                    var msgRect = futureRect.SubRect(futureRect.width - iconSize, iconSize, iconSize);
                    content.text = null;
                    GUI.Box(futureRect, GUIContent.none, style);
                    GUI.Box(iconRect, content);
                    EditorGUI.LabelField(msgRect, msg);
                }
            }
        }

        private void DrawCutsceneHeader(int totalLines) {
            var isEmpty = totalLines == 0;
            //No token in cutscene
            if (isEmpty) {
                EditorGUILayout.LabelField(NoTokenContent, ShiroiStyles.Error);
            }
            //Add button
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(AddTokenContent)) {
                PopupWindow.Show(GUILayoutUtility.GetLastRect(), TokenSelector);
            }
            if (!isEmpty) {
                if (GUILayout.Button(ClearCutscene)) {
                    cutscene.Clear();
                }
            }
            EditorGUILayout.EndHorizontal();
            if (isEmpty) {
                GUI.enabled = false;
                EditorGUILayout.LabelField(GetKaomoji(), ShiroiStyles.Kaomoji, GUILayout.ExpandHeight(true));
            } else {
                //Reload kaomojis
                currentKaomoji = Random.Range(0, Kaomojis.Length - 1);
            }
        }

        private void DrawBackground(Rect rect, int index, bool isactive, bool isfocused) {
            if (index == -1) {
                return;
            }
            var m = MappedToken.For(cutscene[index]);
            var style = isfocused ? m.SelectedStyle : m.Style;
            GUI.Box(rect, GUIContent.none, style);
        }

        private void DrawTokensHeader(Rect rect) {
            EditorGUI.LabelField(rect, "Tokens", ShiroiStyles.Header);
        }

        private float CalculateHeight(int index) {
            var token = cutscene[index];
            return MappedToken.For(token).Height;
        }

        private void DrawToken(Rect rect, int index, bool isactive, bool isfocused) {
            var token = cutscene[index];
            var labelRect = GetRect(rect, 0);
            var mappedToken = MappedToken.For(token);
            EditorGUI.LabelField(labelRect, mappedToken.Label, ShiroiStyles.Bold);
            bool changed;
            mappedToken.DrawFields(this, rect, index, token, cutscene, Player, out changed);
            if (!changed) {
                return;
            }
            EditorUtility.SetDirty(cutscene);
            var l = token as ITokenChangedListener;
            if (l != null) {
                l.OnChanged(cutscene);
            }
        }


        public static Rect GetRect(Rect rect, int index) {
            var x = rect.x;
            var y = rect.y;
            return new Rect(x, y + index * EditorGUIUtility.singleLineHeight, rect.width,
                EditorGUIUtility.singleLineHeight);
        }

        public void AddToken(Type type) {
            cutscene.AddToken((IToken) Activator.CreateInstance(type));
            EditorUtility.SetDirty(this);
            SetCutsceneDirty();
        }

        private void SetCutsceneDirty() {
            EditorUtility.SetDirty(cutscene);
        }
    }
}