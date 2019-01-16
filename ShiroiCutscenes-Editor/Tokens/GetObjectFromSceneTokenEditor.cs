using System;
using System.Collections.Generic;
using Lunari.Tsuki;
using Shiroi.Cutscenes.Editor.Communication;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Tokens;
using UnityEditor;
using UnityEngine;

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
            var t = token.Type;
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField("Object Type", GUILayout.Width(EditorGUIUtility.labelWidth));
                var content = t != null
                    ? new GUIContent(t.Name, ShiroiEditorUtil.GetIconFor(t))
                    : new GUIContent("Selects which kind of object you wish to use");

                var r = EditorGUILayout.DropdownButton(
                    content,
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
            OutputDrawer.Draw(new GUIContent("Output"), ref on, EditorGUILayout.GetControlRect(), t);
            token.OutputName = on;
            if (t != null) {
                token.ActiveObject = EditorGUILayout.ObjectField("Scene Object", token.ActiveObject, t, true);
            } else {
                EditorGUILayout.HelpBox("Please select an object type before selecting an object.", MessageType.Error);
            }
        }
    }
}