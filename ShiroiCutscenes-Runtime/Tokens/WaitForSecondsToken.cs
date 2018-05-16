using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class WaitForSecondsToken : Token {
        public float Duration;
        public bool Realtime;

        public override IEnumerator Execute(CutscenePlayer player) {
            if (Realtime) {
                yield return new WaitForSecondsRealtime(Duration);
            } else {
                yield return new WaitForSeconds(Duration);
            }
        }
    }
}