using System;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor {
    public static class ShiroiEditorUtil {
        public const string CSIcon = "dll Script Icon";

        public static Texture GetIconFor(Type type) {
            var tex = EditorGUIUtility.ObjectContent(null, type).image;
            if (tex == null) {
                tex = EditorGUIUtility.IconContent(CSIcon).image;
            }

            return tex;
        }
    }
}