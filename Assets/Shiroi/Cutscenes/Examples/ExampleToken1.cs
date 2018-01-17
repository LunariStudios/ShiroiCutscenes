using System.Collections;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes.Examples {
    public class ExampleToken1 : IToken {
        public AnimationCurve AccelerationGraph;
        public Color CharacterColor;

        public IEnumerator Execute(CutscenePlayer player) {
            yield break;
        }
    }
}