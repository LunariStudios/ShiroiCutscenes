using System;
using System.Collections.Generic;
using Shiroi.Cutscenes.Tokens;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shiroi.Cutscenes.Editor {
    [CustomEditor(typeof(Cutscene))]
    public class CutsceneEditor : UnityEditor.Editor {
        public static readonly Vector2 TokenWindowSize = new Vector2(150, 500);
        private static GUIStyle baseStyle;
        private static GUIStyle headerStyle;
        public static GUIStyle errorStyle;
        private static GUIStyle kaomojiStyle;
        private static GUIStyle boldStyle;
        private const int KaomojiVerticalBorder = 50;
        private const int KaomojiHorizontalBorder = 50;
        private const int KaomojiFontSize = 100;
        private static RectOffset kaomojiOffset;
        private static int currentKaomoji;
        public TokenSelectorWindow TokenSelector;

        private static readonly GUIContent ClearCutscene =
            new GUIContent("Clear cutscene", "Removes all tokens");

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

        private static void LoadStyles() {
            kaomojiOffset = new RectOffset(
                KaomojiHorizontalBorder,
                KaomojiHorizontalBorder,
                KaomojiVerticalBorder,
                KaomojiVerticalBorder
            );
            baseStyle = new GUIStyle {
                alignment = TextAnchor.MiddleCenter
            };
            boldStyle = new GUIStyle {
                fontStyle = FontStyle.Bold
            };
            headerStyle = new GUIStyle(baseStyle) {
                fontStyle = FontStyle.Bold
            };
            errorStyle = new GUIStyle(baseStyle) {
                fontStyle = FontStyle.Bold,
            };
            kaomojiStyle = new GUIStyle(errorStyle) {
                fontSize = KaomojiFontSize,
                margin = kaomojiOffset,
            };
        }

        private ReorderableList tokenList;

        //Can't use uint because Unity and List indexing uses int:( REEEEEE

        private Cutscene cutscene;

        private void OnEnable() {
            LoadStyles();
            TokenSelector = new TokenSelectorWindow(this);
            cutscene = (Cutscene) target;
            tokenList = new ReorderableList(cutscene.Tokens, typeof(IToken), true, true, false, true) {
                drawHeaderCallback = DrawTokensHeader,
                drawElementCallback = DrawToken,
                drawElementBackgroundCallback = DrawBackground,
                elementHeightCallback = CalculateHeight,
                //onRemoveCallback = OnRemoveCallback,
                //onReorderCallback = OnReorderCallback
            };
        }


        public override void OnInspectorGUI() {
            var totalTokens = cutscene.Tokens.Count;
            DrawCutsceneHeader(totalTokens);
            if (totalTokens <= 0) {
                return;
            }
            tokenList.DoLayoutList();
        }

        private void DrawCutsceneHeader(int totalLines) {
            var isEmpty = totalLines == 0;
            //No token in cutscene
            if (isEmpty) {
                EditorGUILayout.LabelField(NoTokenContent, errorStyle);
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
                EditorGUILayout.LabelField(GetKaomoji(), kaomojiStyle, GUILayout.ExpandHeight(true));
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
            EditorGUI.LabelField(rect, "Tokens", headerStyle);
        }

        private float CalculateHeight(int index) {
            var token = cutscene[index];
            return MappedToken.For(token).Height;
        }

        private void DrawToken(Rect rect, int index, bool isactive, bool isfocused) {
            var token = cutscene[index];
            EditorGUI.LabelField(GetRect(rect, 0), token.GetType().Name, boldStyle);
            MappedToken.For(token).DrawFields(rect, token, cutscene);
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