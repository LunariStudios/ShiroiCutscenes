using System.Collections;
using JetBrains.Annotations;
using Shiroi.Cutscenes.Util;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class DeleteToken : IToken {
        public Reference<Object> Object;

        public IEnumerator Execute(CutscenePlayer player) {
            var obj = Object.Resolve(player);
            if (obj != null) {
                UnityEngine.Object.Destroy(obj);
            }
            yield break;
        }
    }
}