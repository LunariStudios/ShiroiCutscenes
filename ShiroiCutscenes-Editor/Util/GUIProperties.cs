using System;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Util {
    public class GUIColorScope : IDisposable {
        private readonly Color color;

        public GUIColorScope(Color red, bool apply = true) {
            color = GUI.color;
            if (apply) {
                GUI.color = red;
            }
        }

        public void Dispose() {
            GUI.color = color;
        }
    }

}