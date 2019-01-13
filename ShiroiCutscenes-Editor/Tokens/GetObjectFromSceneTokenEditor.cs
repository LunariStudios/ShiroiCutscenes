using System;
using System.Collections.Generic;
using System.Linq;
using Lunari.Tsuki;
using Shiroi.Cutscenes.Communication;
using Shiroi.Cutscenes.Editor.Communication;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Editor.Tokens {
    [CustomEditor(typeof(GetObjectFromSceneToken))]
    public class GetObjectFromSceneTokenEditor : UnityEditor.Editor {
        private readonly List<Type> validObjectTypes = new List<Type>();
        private static readonly GUIContent SelectObjectContent = new GUIContent("Select type");
        private GetObjectFromSceneToken token;
        private TypeSelectorPopupContent<Component> components;
        private Rect lastButtonRect;

        private void OnEnable() {
            token = target as GetObjectFromSceneToken;
            foreach (var type in TypeUtility.GetAllTypesOf<Component>()) {
                validObjectTypes.Add(type);
            }

            validObjectTypes.Add(typeof(GameObject));
            components = new TypeSelectorPopupContent<Component>(
                () => lastButtonRect.width,
                type => {
                    token.Type = type;
                    Repaint();
                },
                (type, root) => type.Namespace != null ? root.FindSubGroup(type.Namespace.Replace('.', '/')) : null);
        }

        public override void OnInspectorGUI() {
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField("Object Type", GUILayout.Width(EditorGUIUtility.labelWidth));
                var r = EditorGUILayout.DropdownButton(
                    new GUIContent(token.Type.Name, ShiroiEditorUtil.GetIconFor(token.Type)),
                    FocusType.Passive
                );
                if (Event.current.type == EventType.Repaint) {
                    lastButtonRect = GUILayoutUtility.GetLastRect();
                }
                if (r) {
                    PopupWindow.Show(lastButtonRect, components);
                }
            }

            var on = token.OutputName;
            OutputDrawer.Draw(new GUIContent("Output"), ref on, EditorGUILayout.GetControlRect(), token.Type);
            token.OutputName = on;
            token.ActiveObject = EditorGUILayout.ObjectField("Scene Object", token.ActiveObject, token.Type, true);
        }
    }
}