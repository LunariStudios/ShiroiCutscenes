using System;
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

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            switch (Type) {
                case DebugType.Info:
                    Debug.Log(Text, this);
                    break;
                case DebugType.Warning:
                    Debug.LogWarning(Text, this);
                    break;
                case DebugType.Error:
                    Debug.LogError(Text, this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield break;
        }
    }
}