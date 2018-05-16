using System.Collections;
using JetBrains.Annotations;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes.Examples {
    [UsedImplicitly]
    public class WaitForInputToken : IToken {
        public string InputKey = "Submit";

        public IEnumerator Execute(CutscenePlayer player) {
            while (!Input.GetButtonDown(InputKey)) {
                yield return null;
            }
        }
    }
}