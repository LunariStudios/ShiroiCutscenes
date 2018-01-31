using UnityEditor;

namespace Shiroi.Cutscenes.Editor.Config {
    [InitializeOnLoad]
    public static class Configs {
        public static readonly BooleanConfig CheckErrors = new BooleanConfig(
            "editor.checkErrors",
            "Check Errors",
            "Whether or not to check token values for possible errors.",
            true);

        public static readonly BooleanConfig ColorfulTokens = new BooleanConfig(
            "editor.colorfulTokens",
            "Colorful Tokens",
            "Whether or not to generate different colors for tokens based on their name, or just use plain grey.",
            true);

        public static readonly Config[] AllConfigs = {
            CheckErrors,
            ColorfulTokens
        };

        static Configs() {
            ColorfulTokens.OnChanged += delegate {
                MappedToken.Clear();
                foreach (var editor in CutsceneEditor.Editors) {
                    editor.Repaint();
                }
            };
        }
    }
}