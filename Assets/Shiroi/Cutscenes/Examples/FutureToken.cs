using System.Collections;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes.Examples {
    public class FutureToken : IToken {
        [FutureType(typeof(Camera))]
        public FutureReference Camera;

        public IEnumerator Execute(CutscenePlayer player) {
            var camera = Camera.Resolve<Camera>(player);
            yield break;
        }
    }
}