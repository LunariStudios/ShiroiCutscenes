using System.Collections;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes.Examples {
    public class ExampleToken1 : IToken {
        public AnimationCurve AccelerationGraph = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public Color CharacterColor = Color.red;

        public IEnumerator Execute(CutscenePlayer player) {
            yield break;
        }
    }
}