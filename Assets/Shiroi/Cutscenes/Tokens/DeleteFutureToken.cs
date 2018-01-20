using System.Collections;
using JetBrains.Annotations;
using Shiroi.Cutscenes.Futures;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class DeleteFutureToken : IToken {
        public FutureReference Future;

        public IEnumerator Execute(CutscenePlayer player) {
            var obj = Future.Resolve<Object>(player);
            if (obj != null) {
                Object.Destroy(obj);
            }
            yield break;
        }
    }
}