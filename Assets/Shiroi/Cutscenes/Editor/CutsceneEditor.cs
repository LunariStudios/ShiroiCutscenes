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
        public static readonly Vector2 tokenWindowSize = new Vector2(150, 500);
        private static GUIStyle baseStyle;
        private static GUIStyle headerStyle;
        private static GUIStyle errorStyle;
        private static GUIStyle kaomojiStyle;
        private static GUIStyle boldStyle;
        private const int KaomojiVerticalBorder = 50;
        private const int KaomojiHorizontalBorder = 50;
        private const int KaomojiFontSize = 100;
        private static RectOffset kaomojiOffset;
        private static int currentKaomoji;
        private static List<GUIStyle> TokensStyles = new List<GUIStyle>();

        private static readonly GUIContent addTokenContent =
            new GUIContent("Choose your flavour", "Add a token to the current line");

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

        private Dictionary<int, ReorderableList> listCache;

        private ReorderableList currentPage;

        //Can't use uint because Unity and List indexing uses int:( REEEEEE
        private int currentLine;

        private CutsceneLine CurrentLine {
            get { return cutscene[currentLine]; }
        }

        private Cutscene cutscene;

        private void OnEnable() {
            LoadStyles();
            currentLine = 0;
            cutscene = (Cutscene) target;
            listCache = new Dictionary<int, ReorderableList>();
            ReloadTokenStyles();
        }


        public override void OnInspectorGUI() {
            var totalLines = cutscene.Lines.Count;
            DrawCutsceneHeader(totalLines);
            if (totalLines <= 0) {
                return;
            }
            if (currentPage == null) {
                currentPage = GetPage(currentLine);
            }
            currentPage.DoLayoutList();
        }

        private void DrawCutsceneHeader(int totalLines) {
            //First line
            var isEmpty = totalLines == 0;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Before")) {
                cutscene.InsertLine(currentLine);
                if (!isEmpty) {
                    currentLine++;
                }
                SetCutsceneDirty();
            }
            GUI.enabled = !cutscene.IsEmpty;
            if (GUILayout.Button("Remove Current")) {
                DoRemoveLine(currentLine, totalLines);
                SetCutsceneDirty();
                //May be empty now
                totalLines--;
                isEmpty = totalLines == 0;
            }
            if (GUILayout.Button("Add After")) {
                cutscene.InsertLine(currentLine + 1);
                SetCutsceneDirty();
            }
            EditorGUILayout.EndHorizontal();
            //Second line
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = currentLine > 0;
            if (GUILayout.Button("Previous")) {
                currentLine--;
            }
            GUI.enabled = true;
            if (isEmpty) {
                EditorGUILayout.LabelField("There aren't any cutscene lines defined!", errorStyle);
            } else {
                EditorGUILayout.LabelField("Editing line " + (currentLine + 1) + "/" + totalLines, headerStyle);
            }
            GUI.enabled = currentLine < totalLines - 1;
            if (GUILayout.Button("Next")) {
                currentLine++;
            }
            EditorGUILayout.EndHorizontal();
            if (isEmpty) {
                GUI.enabled = false;
                EditorGUILayout.LabelField(GetKaomoji(), kaomojiStyle, GUILayout.ExpandHeight(true));
            } else {
                DrawCutsceneLineInfo();
                //Reload kaomojis
                currentKaomoji = Random.Range(0, Kaomojis.Length - 1);
            }
        }

        private void DrawCutsceneLineInfo() {
            GUI.enabled = true;
            CurrentLine.Delay = EditorGUILayout.FloatField("Delay: ", CurrentLine.Delay);
            if (GUILayout.Button(addTokenContent)) {
                var window = EditorWindow.GetWindow<TokenSelectorWindow>();
                window.CurrentEditor = this;
                window.ShowUtility();
            }
        }

        private void DoRemoveLine(int index, int totalLines) {
            cutscene.RemoveLine(currentLine);
            if (listCache.ContainsKey(currentLine)) {
                listCache.Remove(currentLine);
            }
            if (currentLine != 0 && currentLine == totalLines - 1) {
                currentLine--;
            }
        }

        private ReorderableList GetPage(int index) {
            ReorderableList list;
            if (listCache.TryGetValue(index, out list)) {
                return list;
            }
            list = CreateNewList(index);
            listCache[index] = list;
            return list;
        }

        private ReorderableList CreateNewList(int index) {
            var list = CreateList(index);
            listCache[index] = list;
            return list;
        }

        private ReorderableList CreateList(int index) {
            return new ReorderableList(cutscene.Lines[index].Tokens, typeof(IToken), true, true, false, true) {
                drawHeaderCallback = DrawTokensHeader,
                drawElementCallback = DrawToken,
                drawElementBackgroundCallback = DrawBackground,
                elementHeightCallback = CalculateHeight,
                //onRemoveCallback = OnRemoveCallback,
                //onReorderCallback = OnReorderCallback
            };
        }

        private void DrawBackground(Rect rect, int index, bool isactive, bool isfocused) {
            if (index == -1) {
                return;
            }
            var style = GetTokenStyle(index);
            GUI.Box(rect, GUIContent.none, style);
        }

        private GUIStyle GetTokenStyle(int index) {
            return TokensStyles[index];
        }

        private void OnReorderCallback(ReorderableList list) { }

        private void OnRemoveCallback(ReorderableList reorderableList) {
            CurrentLine.RemoveToken(reorderableList.index);
            ReloadTokenStyles();
        }


        private void DrawTokensHeader(Rect rect) {
            EditorGUI.LabelField(rect, "Tokens", headerStyle);
        }

        private float CalculateHeight(int index) {
            var token = CurrentLine[index];
            return MappedToken.For(token).Height;
        }

        private void DrawToken(Rect rect, int index, bool isactive, bool isfocused) {
            var token = CurrentLine[index];
            EditorGUI.LabelField(GetRect(rect, 0), token.GetType().Name, boldStyle);
            MappedToken.For(token).DrawFields(rect, token);
        }


        public static Rect GetRect(Rect rect, int index) {
            var x = rect.x;
            var y = rect.y;
            return new Rect(x, y + index * EditorGUIUtility.singleLineHeight, rect.width,
                EditorGUIUtility.singleLineHeight);
        }

        public void AddToken(Type type) {
            CurrentLine.AddToken((IToken) Activator.CreateInstance(type));
            ReloadTokenStyles();
            EditorUtility.SetDirty(this);
            SetCutsceneDirty();
        }

        private void ReloadTokenStyles() {
            TokensStyles.Clear();
            var max = CurrentLine.Tokens.Count;
            for (var i = 0; i < max; i++) {
                var style = new GUIStyle(GUI.skin.box);
                var hue = (float) i / max;
                var componentColor = Color.HSVToRGB(hue, 0.7f, 1f);
                style.normal.background = CreateTexture(2, 2, componentColor);
                TokensStyles.Add(style);
            }
        }

        //Sorry entitas
        public static Texture2D CreateTexture(int width, int height, Color color) {
            var pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; ++i) {
                pixels[i] = color;
            }
            var result = new Texture2D(width, height);
            result.SetPixels(pixels);
            result.Apply();
            return result;
        }

        private void SetCutsceneDirty() {
            EditorUtility.SetDirty(cutscene);
        }
    }
}