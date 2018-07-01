using System;
using Shiroi.Cutscenes.Editor.Errors;
using Shiroi.Cutscenes.Tokens;
using UnityEditor;
using UnityEngine;
using UnityUtilities.Editor;

namespace Shiroi.Cutscenes.Editor.Cutscenes {
    [CustomEditor(typeof(Cutscene))]
    public partial class CutsceneEditor : UnityEditor.Editor {
        public Cutscene Cutscene {
            get;
            private set;
        }

        public CutscenePlayer Player {
            get;
            private set;
        }

        public ErrorManager ErrorManager {
            get {
                //TODO: Fix
                throw new NotImplementedException();
            }
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
            EditorUtility.SetDirty(this);
            //TokenList.index = Cutscene.Count - 1;
        }
    }
}