using UnityEditor;

namespace Shiroi.Cutscenes.Editor.Config {
    [InitializeOnLoad]
    public static class Configs {
        public static readonly BooleanConfig CheckErrors = new BooleanConfig(
            "editor.checkTokenErrors",
            "Check Token Errors",
            "Whether or not to check token values for possible errors.",
            true);

        public static readonly BooleanConfig ErrorColors = new BooleanConfig(
            "editor.errorColor",
            "Colorful Errors",
            "If everything should go colored when an error is detected",
            true);

        public static readonly BooleanConfig ErrorIcons = new BooleanConfig(
            "editor.errorIcon",
            "Error Icon",
            "If everything should go red when an error is detected",
            true);

        public static readonly BooleanConfig ColorfulTokens = new BooleanConfig(
            "editor.colorfulTokens",
            "Colorful Tokens",
            "Whether or not to generate different colors for tokens based on their name, or just use plain grey.",
            true
        );

        public static readonly IntegerConfig TokensPerPage = new IntegerConfig(
            "editor.tokensPerPage",
            "Tokens Per Page",
            "The max number of tokens to display per page",
            10
        );

        public static readonly Config[] AllConfigs = {
            CheckErrors,
            ColorfulTokens,
            ErrorColors,
            ErrorIcons,
            TokensPerPage
        };

        static Configs() {
            ColorfulTokens.OnChanged += delegate {
                MappedToken.Clear();
                RepaintEditors();
            };
            ErrorColors.OnChanged += delegate {
                ShiroiStyles.Reload();
                RepaintEditors();
            };
            ErrorIcons.OnChanged += delegate {
                ShiroiStyles.Reload();
                RepaintEditors();
            };
        }

        private static void RepaintEditors() {
            /*
             TODO: Fix
             foreach (var editor in CutsceneEditor.Editors) {
                editor.Repaint();
            }
            */
        }
    }
}