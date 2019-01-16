using System.Collections;
using JetBrains.Annotations;
using System.ComponentModel;
using Shiroi.Cutscenes.Communication;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    [Category(ShiroiCutscenesConstants.CommonCategory)]
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