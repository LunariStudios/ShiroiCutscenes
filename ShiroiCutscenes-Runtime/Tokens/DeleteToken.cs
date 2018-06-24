using System.Collections;
using JetBrains.Annotations;
using Shiroi.Cutscenes.Util;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class DeleteToken : Token {
        public Reference<Object> Object;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var obj = Object.Resolve(player);
            if (obj != null) {
                Destroy(obj);
            }
            yield break;
        }
    }
}