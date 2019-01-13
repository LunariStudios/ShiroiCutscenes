using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunari.Tsuki;
using Lunari.Tsuki.Editor;
using Shiroi.Cutscenes.Attributes;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Util {
    public class TypeSelectorGroup {
        public const float ItemHeight = 16f;

        public static void DrawItems(List<Type> items, Rect r, Action<Type> onTypeSelected) {
            r.height = ItemHeight;
            foreach (var type in items) {
                if (GUI.Button(r, new GUIContent(type.GetNiceName(), ShiroiEditorUtil.GetIconFor(type)),
                    ItemStyle.Value)) {
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
                    padding = new RectOffset(5, 5, 5, 5)
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
                    padding = new RectOffset(5, 5, 5, 5)
                };
                return style;
            });

        private float CalcHeight(float width) {
            float totalHeight = 0;
            var groupStyle = GroupStyle.Value;
            var itemStyle = ItemStyle.Value;
            foreach (var subgroup in Subgroups) {
                totalHeight += groupStyle.CalcHeight(new GUIContent($"{subgroup.Name}/"), width);
            }

            foreach (var type in GroupTypes) {
                var content = new GUIContent(type.GetNiceName(), ShiroiEditorUtil.GetIconFor(type));
                totalHeight += itemStyle.CalcHeight(content, width);
            }

            return totalHeight;
        }

        public void Draw<T>(Rect rect, TypeSelectorPopupContent<T> selector,
            Action<Type> callback) {
            var viewRect = rect.SubXMax(16);
            var groupStyle = GroupStyle.Value;
            var totalHeight = CalcHeight(viewRect.width);
            var itemRect = viewRect;
            viewRect.height = totalHeight;
            using (var scope = new GUI.ScrollViewScope(rect, scrollPos, viewRect)) {
                scrollPos = scope.scrollPosition;
                foreach (var group in Subgroups) {
                    var msg = $"{group.Name}/";
                    var content = new GUIContent(msg);
                    var height = groupStyle.CalcHeight(content, itemRect.width);
                    itemRect.height = height;
                    var fwdArrow = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector)
                        .GetStyle(GUISkinProperties.ACRightArrow);
                    if (Event.current.type == EventType.Repaint) {
                        fwdArrow.Draw(itemRect.SetXMin(itemRect.xMax - itemRect.height), false, false, false, false);
                    }

                    if (GUI.Button(itemRect, content, groupStyle)) {
                        selector.Path.Push(group.Name);
                    }

                    itemRect.y += height;
                }

                DrawItems(GroupTypes, itemRect, callback);
            }
        }

        public List<Type> SearchTypes(string searchString) {
            var types = new List<Type>();
            foreach (var subgroup in Subgroups) {
                types.AddRange(subgroup.SearchTypes(searchString));
            }

            // Add the ones that start with first
            foreach (var groupType in GroupTypes) {
                if (groupType.Name.StartsWith(searchString, StringComparison.InvariantCultureIgnoreCase) ||
                    groupType.Name.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) >= 0) {
                    types.Add(groupType);
                }
            }

            types.Sort((type, t) => {
                var nameA = type.GetNiceName();
                var nameB = t.GetNiceName();
                var nas = nameA.StartsWith(searchString, StringComparison.InvariantCultureIgnoreCase);
                var nbs = nameB.StartsWith(searchString, StringComparison.InvariantCultureIgnoreCase);
                if (nas && nbs) {
                    return 0;
                }

                if (nas) {
                    return -1;
                }

                return nbs ? 1 : 0;
            });

            return types;
        }
    }

    public class TypeSelectorPopupContent<T> : PopupWindowContent {
        public const float SearchHeight = 24;
        public const string DefaultNoElementsFoundMessage = "No items found :(";
        private readonly Action<Type> onTypeSelected;
        private readonly TypeSelectorGroup rootGroup;
        private readonly Func<float> widthCalculator;
        private readonly string noElementsFoundMessage;

        public TypeSelectorPopupContent(Func<float> widthCalculator,
            Action<Type> onTypeSelected,
            Func<Type, TypeSelectorGroup, TypeSelectorGroup> groupSelector = null,
            string noElementsFoundMessage = DefaultNoElementsFoundMessage) {
            this.onTypeSelected = onTypeSelected;
            this.widthCalculator = widthCalculator;
            this.noElementsFoundMessage = noElementsFoundMessage;
            rootGroup = new TypeSelectorGroup("Root");
            var types = TypeUtility.GetAllTypesOf<T>().Where(type => !type.IsAbstract).ToList();
            foreach (var type in types) {
                var a = (TokenCategoryAttribute) type.GetCustomAttributes(typeof(TokenCategoryAttribute), true)
                    .FirstOrDefault();
                TypeSelectorGroup group = null;
                if (a == null) {
                    if (groupSelector != null) {
                        group = groupSelector(type, rootGroup);
                    }
                } else {
                    group = rootGroup.FindSubGroup(a.Category);
                }

                group = group ?? rootGroup;
                group.GroupTypes.Add(type);
            }
        }

        public override Vector2 GetWindowSize() {
            return new Vector2(widthCalculator(), 500);
        }

        public readonly Stack<string> Path = new Stack<string>();

        public static readonly Lazy<GUIStyle> PathStyle = new Lazy<GUIStyle>(
            () => new GUIStyle {
                alignment = TextAnchor.MiddleCenter
            });

        private string searchString = string.Empty;
        private readonly SearchField searchField = new SearchField();
        private Vector2 searchScroll;

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
                var viewRect = groupRect.SubXMax(16);
                var foundTypes = rootGroup.SearchTypes(searchString);
                viewRect.height = foundTypes.Sum(type
                    => TypeSelectorGroup.ItemStyle.Value.CalcHeight(
                        new GUIContent(type.GetNiceName(), ShiroiEditorUtil.GetIconFor(type)), viewRect.width));
                using (var scope = new GUI.ScrollViewScope(groupRect, searchScroll, viewRect)) {
                    searchScroll = scope.scrollPosition;
                    TypeSelectorGroup.DrawItems(foundTypes, viewRect, onTypeSelected);
                }
            } else {
                // Draw Tree
                searchScroll = Vector2.zero;

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