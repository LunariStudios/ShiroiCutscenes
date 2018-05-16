using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class DebugTextToken : Token {
        public enum DebugType {
            Info,
            Warning,
            Error
        }

        public string Text;
        public DebugType Type = DebugType.Info;

        public override IEnumerator Execute(CutscenePlayer player) {
            Debug.Log(Text);
            yield break;
        }
    }
}