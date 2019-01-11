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
            GameObject obj, p;
            if (Object.Get(executor.Context, out obj) && NewParent.Get(executor.Context, out p)) {
                obj.transform.SetParent(p.transform);
            }

            yield break;
        }
    }
}