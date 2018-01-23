using System.Collections;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes.Examples {
    public class FutureToken : IToken {
        public FutureReference<GameObject> Object;

        public IEnumerator Execute(CutscenePlayer player) {
            var camera = Object.Resolve(player);
            Debug.Log("Resolved future object = " + camera);
            yield break;
        }
    }
}