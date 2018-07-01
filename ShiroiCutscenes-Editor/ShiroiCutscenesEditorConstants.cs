using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor {
    public static class ShiroiCutscenesEditorConstants {
        public static readonly GUIContent EditorBoundPlayer = new GUIContent(
            "Bound Player",
            "The bound player is the CutscenePlayer that the references of this cutscene is currently being recorded to"
        );

        public static readonly GUIContent CutsceneEditorHeader = new GUIContent(
            "Cutscene Metadata",
            "Information about the cutscene editor, bound player, futures or any kind of meta data about the current" +
            " cutscene is displayed here."
        );

        public static readonly GUIContent NoPlayerBoundContent = new GUIContent(
            "When a CutscenePlayer becomes bound, it's meta will be displayed here"
        );

        public static readonly GUIContent CutsceneEditorTokensHeader = new GUIContent(
            "Tokens",
            "All information about the tokens that structure this cutscenes"
        );

        public static readonly GUIContent AddTokenContent = new GUIContent(
            "Choose your flavour",
            "Opens a popup to add tokens to this cutscenes");

        public static readonly GUIContent ClearCutsceneContent = new GUIContent(
            "Clear Cutscene",
            "Removes all the tokens from this cutscene"
        );

        public static readonly GUIContent SettingsButtonContent = new GUIContent(
            "Settings",
            "Open settings for the ShiroiCutscenes Editor"
        );

        public static readonly GUIContent FuturesHeaderContent = new GUIContent(
            "Futures",
            "Information about futures will be displayed here"
        );

        public static readonly GUIContent EmptyFuturesContent = new GUIContent(
            "There are no futures on this cutscene",
            "This isn't bad by the way, we're just letting you know");

        public const float IconSize = 20;

        public const float ConfigsSpacing = 10;
    }
}