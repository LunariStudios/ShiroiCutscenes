using System.Collections;
using JetBrains.Annotations;
using Shiroi.Cutscenes.Futures;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class DeleteFutureToken : IToken {
        public FutureReference<Object> Future;

        public IEnumerator Execute(CutscenePlayer player) {
            var obj = Future.Resolve(player);
            if (obj != null) {
                Object.Destroy(obj);
            }
            yield break;
        }
    }
}