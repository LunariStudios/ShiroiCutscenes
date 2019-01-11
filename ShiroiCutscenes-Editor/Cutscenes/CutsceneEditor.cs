using System;
using Lunari.Tsuki.Editor;
using Shiroi.Cutscenes.Tokens;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Cutscenes {
    [CustomEditor(typeof(Cutscene))]
    public partial class CutsceneEditor : UnityEditor.Editor {
        public Cutscene Cutscene {
            get;
            private set;
        }

        private void OnEnable() {
            Cutscene = (Cutscene) target;
            TokensOnEnable();
        }

        public override void OnInspectorGUI() {
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            DrawCutsceneHeader(skin);
            DrawTokens(skin);
        }

        public void AddToken(Type type) {
            var instance = (Token) Cutscene.AddToAssetFile(type);
            instance.name = type.Name;
            /*if (Cutscene.IsEmpty || TokenList.index < 0) {
                Cutscene.Add(instance);
            } else {
                Cutscene.Add(TokenList.index, instance);
            }*/
            Cutscene.Add(instance);
            ReloadSubEditors();
            Repaint();
            //TokenList.index = Cutscene.Count - 1;
        }

        public static readonly GUIContent EditorBoundPlayer = new GUIContent(
            "Bound Player",
            "The bound player is the CutscenePlayer that the references of this cutscene is currently being recorded to"
        );

        public static readonly GUIContent AddTokenContent = new GUIContent(
            "Choose your flavour",
            "Opens a popup to add tokens to this cutscenes");

        public static readonly GUIContent FuturesHeaderContent = new GUIContent(
            "Futures",
            "Information about futures will be displayed here"
        );
    }
}