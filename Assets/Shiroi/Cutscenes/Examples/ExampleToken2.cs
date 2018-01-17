using System.Collections;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes.Examples {
    public class ExampleToken2 : IToken {
        public Quaternion Rotation;

        public IEnumerator Execute(CutscenePlayer player) {
            yield break;
        }
    }
}