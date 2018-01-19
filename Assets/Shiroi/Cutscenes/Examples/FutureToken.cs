using System.Collections;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes.Examples {
    public class FutureToken : IToken {
        [FutureType(typeof(GameObject))]
        public FutureReference Object;

        public IEnumerator Execute(CutscenePlayer player) {
            var camera = Object.Resolve<GameObject>(player);
            Debug.Log("Resolved future object = " + camera);
            yield break;
        }
    }
}