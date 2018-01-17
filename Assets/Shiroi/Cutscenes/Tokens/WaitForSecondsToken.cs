using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class WaitForSecondsToken : IToken {
        public float Duration;
        public bool Realtime;

        public IEnumerator Execute(CutscenePlayer player) {
            if (Realtime) {
                yield return new WaitForSecondsRealtime(Duration);
            } else {
                yield return new WaitForSeconds(Duration);
            }
        }
    }
}