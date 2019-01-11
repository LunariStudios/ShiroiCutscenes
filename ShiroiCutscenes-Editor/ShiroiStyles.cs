using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor {
    [InitializeOnLoad]
    public static class ShiroiStyles {

        public static readonly GUIStyle NoMargin = new GUIStyle {
            border = new RectOffset(),
            margin = new RectOffset(),
            stretchWidth = false
        };


        public static readonly string[] Kaomojis = {
            ":(",
            "(^-^*)",
            "(;-;)",
            "(o^^)o",
            "(>_<)",
            "\\(^Д^)/",
            "(≥o≤)",
            "(·.·)",
            "╯°□°）╯︵ ┻━┻",
            "(✖╭╮✖)",
            "(⌣_⌣”)"
        };
    }
}