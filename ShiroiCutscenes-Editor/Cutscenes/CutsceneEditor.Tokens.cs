using System;
using System.Collections.Generic;
using System.Linq;
using Lunari.Tsuki;
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
        private CutsceneTokenSelectorPopupContent tokenPopUpContent;
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
                            EditorGUILayout.LabelField(string.Format("#{0} - {1}", i, token.name),
                                EditorStyles.boldLabel);
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

        private void DrawTokensHeaderButtons(GUISkin skin) {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(
                AddTokenContent,
                GUISkinProperties.LargeButtonMid
            )) {
                if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Used) {
                    var r = GUILayoutUtility.GetLastRect();
                    PopupWindow.Show(r, tokenPopUpContent);
                }
            }

            if (GUILayout.Button(
                ClearCutsceneContent,
                GUISkinProperties.LargeButtonMid
            )) {
                Cutscene.Clear();
                ReloadSubEditors();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void TokensOnEnable() {
            tokenPopUpContent = new CutsceneTokenSelectorPopupContent(() => 200, type => {
                Cutscene.Add((Token) CreateInstance(type));
                ReloadSubEditors();
            });
            ReloadSubEditors();
        }

        private void ReloadSubEditors() {
            subEditors = new List<UnityEditor.Editor>();
            foreach (var token in Cutscene.Tokens) {
                subEditors.Add(CreateEditor(token));
            }
        }
    }

    public class TypeSelectorGroup {
        public const float ItemHeight = 16f;

        public static void DrawItems(List<Type> items, Rect r, Action<Type> onTypeSelected) {
            r.height = ItemHeight;
            foreach (var type in items) {
                if (GUI.Button(r, type.Name, ItemStyle.Value)) {
                    onTypeSelected(type);
                }

                r.y += ItemHeight;
            }
        }

        public string Name;

        public TypeSelectorGroup(string name) {
            Name = name;
        }

        public List<TypeSelectorGroup> Subgroups {
            get;
        } = new List<TypeSelectorGroup>();

        public List<Type> GroupTypes {
            get;
        } = new List<Type>();

        public TypeSelectorGroup FindSubGroup(string path) {
            if (path.IsNullOrEmpty() || path.All(c => c == ' ')) {
                return this;
            }

            var splitPath = path.Split('/').ToList();
            var name = splitPath.First();

            var subgroup = Subgroups.FirstOrAdd(group => @group.Name == name, () => new TypeSelectorGroup(name));
            if (splitPath.Count == 1) {
                return subgroup;
            }

            var newPath = string.Join("/", splitPath.GetRange(1, splitPath.Count - 1).ToArray());
            return subgroup.FindSubGroup(newPath);
        }

        public bool IsEmpty() {
            return Subgroups.IsEmpty() && GroupTypes.IsEmpty();
        }

        private Vector2 scrollPos;

        public static readonly Lazy<GUIStyle> GroupStyle = new Lazy<GUIStyle>(
            () => {
                var style = new GUIStyle {
                    hover = {
                        background = TextureUtility.PixelTexture(new Color32(62, 90, 230, 255)),
                        textColor = Color.white
                    },
                    active = {
                        background = TextureUtility.PixelTexture(new Color32(71, 102, 255, 255)),
                        textColor = Color.white
                    },
                    focused = {
                        background = TextureUtility.PixelTexture(new Color32(62, 90, 230, 255)),
                        textColor = Color.white
                    },
                    fontStyle = FontStyle.Bold,
                    fixedHeight = 24f,
                    alignment = TextAnchor.MiddleCenter,
                };
                return style;
            });

        public static readonly Lazy<GUIStyle> ItemStyle = new Lazy<GUIStyle>(
            () => {
                var style = new GUIStyle() {
                    hover = {
                        background = TextureUtility.PixelTexture(new Color32(62, 125, 231, 255)),
                        textColor = Color.white
                    },
                    active = {
                        background = TextureUtility.PixelTexture(new Color32(69, 140, 255, 255)),
                        textColor = Color.white
                    },
                    focused = {
                        background = TextureUtility.PixelTexture(new Color32(55, 112, 204, 255)),
                        textColor = Color.white
                    },
                    alignment = TextAnchor.MiddleCenter,
                };
                return style;
            });

        public void Draw(Rect rect, CutsceneTokenSelectorPopupContent selector, Action<Type> callback) {
            var viewRect = rect;
            scrollPos = GUI.BeginScrollView(rect, scrollPos, viewRect);
            int i;
            var r = rect;
            var groupStyle = GroupStyle.Value;
            for (i = 0; i < Subgroups.Count; i++) {
                var group = Subgroups[i];
                var msg = $"{group.Name}/";
                var content = new GUIContent(msg);
                var height = groupStyle.CalcHeight(content, r.width);
                r.height = height;
                var fwdArrow = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector)
                    .GetStyle(GUISkinProperties.ACRightArrow);
                if (Event.current.type == EventType.Repaint) {
                    fwdArrow.Draw(r.SetXMin(r.xMax - r.height), false, false, false, false);
                }

                if (GUI.Button(r, content, groupStyle)) {
                    selector.Path.Push(group.Name);
                }

                r.y += height;
            }

            DrawItems(GroupTypes, r, callback);
            GUI.EndScrollView();
        }

        public List<Type> SearchTypes(string searchString) {
            var types = new List<Type>();
            foreach (var subgroup in Subgroups) {
                types.AddRange(subgroup.SearchTypes(searchString));
            }

            foreach (var groupType in GroupTypes) {
                if (groupType.Name.StartsWith(searchString, StringComparison.InvariantCultureIgnoreCase)) {
                    types.Add(groupType);
                }
            }

            return types;
        }
    }

    public class CutsceneTokenSelectorPopupContent : PopupWindowContent {
        public const string DefaultNoElementsFoundMessage = "No items found :(";
        private readonly Action<Type> onTypeSelected;
        private readonly TypeSelectorGroup rootGroup;
        private readonly Func<float> widthCalculator;
        private readonly string noElementsFoundMessage;

        public CutsceneTokenSelectorPopupContent(
            Func<float> widthCalculator,
            Action<Type> onTypeSelected,
            string noElementsFoundMessage = DefaultNoElementsFoundMessage) {
            this.onTypeSelected = onTypeSelected;
            this.widthCalculator = widthCalculator;
            this.noElementsFoundMessage = noElementsFoundMessage;
            rootGroup = new TypeSelectorGroup("Root");
            var types = TypeUtility.GetAllTypesOf<Token>().Where(type => !type.IsAbstract);
            foreach (var type in types) {
                var a = (TokenCategoryAttribute) type.GetCustomAttributes(typeof(TokenCategoryAttribute), true).FirstOrDefault();
                var group = a == null ? rootGroup : rootGroup.FindSubGroup(a.Category);
                group.GroupTypes.Add(type);
            }
        }

        public override Vector2 GetWindowSize() {
            return new Vector2(widthCalculator(), 500);
        }

        public readonly Stack<string> Path = new Stack<string>();

        public void ResetPath() {
            Path.Clear();
        }

        public static readonly Lazy<GUIStyle> PathStyle = new Lazy<GUIStyle>(
            () => new GUIStyle {
                alignment = TextAnchor.MiddleCenter
            });

        private string searchString = string.Empty;
        public const float SearchHeight = 24;
        private readonly SearchField searchField = new SearchField();

        public override void OnGUI(Rect rect) {
            var pStyle = PathStyle.Value;
            var path = string.Join("/", Path.Reverse().ToArray());
            var pathContent = new GUIContent($"/{path}");
            var pathHeight = pStyle.CalcHeight(pathContent, rect.width) + 5;
            GUI.Box(rect.SetHeight(SearchHeight + pathHeight), GUIContent.none);
            var searchRect = rect;
            searchRect.height = SearchHeight;
            searchString = searchField.OnGUI(searchRect.Padding(5), searchString);

            var labelRect = rect;
            labelRect.yMin = searchRect.yMax;
            labelRect.yMax = labelRect.yMin + pathHeight;

            var groupRect = rect;
            groupRect.yMin = labelRect.yMax;
            if (!searchString.IsEmpty()) {
                // Draw Search
                var foundTypes = rootGroup.SearchTypes(searchString);
                TypeSelectorGroup.DrawItems(foundTypes, groupRect, onTypeSelected);
            } else {
                // Draw Tree
                var group = rootGroup.FindSubGroup(path);
                EditorGUI.LabelField(labelRect, pathContent, pStyle);
                if (!IsAtRoot) {
                    var backArrow = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector)
                        .GetStyle(GUISkinProperties.ACLeftArrow);
                    var returnRect = labelRect.SetXMax(labelRect.x + labelRect.height);
                    if (GUI.Button(returnRect, GUIContent.none, backArrow)) {
                        Path.Pop();
                    }

                    //backArrow.Draw(returnRect, false, false, false, false);
                }

                if (group.IsEmpty()) {
                    EditorGUI.LabelField(groupRect, noElementsFoundMessage, EditorStyles.boldLabel);
                } else {
                    group.Draw(groupRect, this, onTypeSelected);
                }
            }

            editorWindow.Repaint();
        }


        public bool IsAtRoot => Path.Count == 0;
    }
}