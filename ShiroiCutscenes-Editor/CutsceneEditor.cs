using System;
using System.Collections.Generic;
using Shiroi.Cutscenes.Editor.Config;
using Shiroi.Cutscenes.Editor.Errors;
using Shiroi.Cutscenes.Editor.Preview;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Editor.Windows;
using Shiroi.Cutscenes.Preview;
using Shiroi.Cutscenes.Tokens;
using UnityEditor;
using UnityEngine;
using UnityUtilities.Editor;
using Random = UnityEngine.Random;

namespace Shiroi.Cutscenes.Editor {
    [CustomEditor(typeof(Cutscene))]
    public class CutsceneEditor : UnityEditor.Editor {
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
        }

        public override void OnInspectorGUI() { }
    }
}