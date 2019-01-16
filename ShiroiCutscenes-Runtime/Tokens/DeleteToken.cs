using System.Collections;
using JetBrains.Annotations;
using System.ComponentModel;
using Shiroi.Cutscenes.Communication;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    [Category(ShiroiCutscenesConstants.CommonCategory)]
    public class DeleteToken : Token {
        public ObjectInput Object;

        public override IEnumerator Execute(CutsceneExecutor executor) {
            Object obj;
            if (Object.Get(executor.Context, out obj)) {
                Destroy(obj);
            }

            yield break;
        }
    }
}