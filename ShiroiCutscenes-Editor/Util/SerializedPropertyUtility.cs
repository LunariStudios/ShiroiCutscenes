using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Util {
    public static class SerializedPropertyUtility {
        public static T FindInstanceWithin<T>(this SerializedProperty property) where T : class {
            return property.FindInstanceAt<T>(property.serializedObject.targetObject);
        }

        public static T FindInstanceAt<T>(this SerializedProperty property, Object obj) where T : class {
            return property.FindInstanceAt(obj) as T;
        }

        private static object ExtractFromField(string fieldName, object owner) {
            var field = owner.GetType().GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            return field?.GetValue(owner);
        }

        public static object FindInstanceAt(this SerializedProperty property, Object obj) {
            var path = property.propertyPath;
            var nodes = path.Split('.');
            var firstProperty = nodes[0];
            var current = ExtractFromField(firstProperty, obj);
            var child = property.serializedObject.FindProperty(firstProperty);
            for (var i = 1; i < nodes.Length; i++) {
                var fieldName = nodes[i];
                if (fieldName.Equals("Array")) {
                    continue;
                }

                object found;

                if (fieldName.StartsWith("data")) {
                    var indexS = fieldName.Replace("data[", string.Empty).Replace("]", string.Empty);
                    var index = int.Parse(indexS);
                    var list = (IList) current;
                    found = list[index];
                } else {
                    child = child.FindPropertyRelative(fieldName);
                    found = ExtractFromField(fieldName, current);
                }

                current = found;
            }

            return current;
        }
    }
}