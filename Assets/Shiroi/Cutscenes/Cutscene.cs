using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shiroi.Cutscenes {
    [CreateAssetMenu(menuName = CreateCutsceneMenuPath), Serializable]
    public class Cutscene : ScriptableObject {
        public const string CreateCutsceneMenuPath = "Shiroi/Cutscenes/Cutscene";
        public List<CutsceneLine> Lines = new List<CutsceneLine>();

        public CutsceneLine this[int index] {
            get {
                return Lines[index];
            }
            set { Lines[index] = value; }
        }

        public bool IsEmpty {
            get { return Lines.Count == 0; }
        }

        public void InsertLine(int index) {
            Lines.Insert(index, new CutsceneLine());
        }

        public void RemoveLine(int currentLine) {
            Lines.RemoveAt(currentLine);
        }
    }
}