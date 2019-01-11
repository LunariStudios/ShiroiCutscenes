using System.Collections;
using JetBrains.Annotations;
using Shiroi.Cutscenes.Attributes;
using Shiroi.Cutscenes.Communication;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    [TokenCategory(ShiroiCutscenesConstants.CommonCategory)]
    public class SetParentToken : Token {
        public GameObjectInput Object;
        public GameObjectInput NewParent;

        public override IEnumerator Execute(CutsceneExecutor executor) {
            if (Object.Get(executor.Context, out var obj) && NewParent.Get(executor.Context, out var p)) {
                obj.transform.SetParent(p.transform);
            }

            yield break;
        }
    }
}