using System.Collections;
using System.ComponentModel;
using Shiroi.Cutscenes.Communication;

namespace Shiroi.Cutscenes.Tokens {
    [Category(ShiroiCutscenesConstants.CommonCategory)]
    public class SetGameObjectActiveToken : Token {
        public GameObjectInput Target;
        public bool Active;

        public override IEnumerator Execute(CutsceneExecutor executor) {
            if (Target.Get(executor.Context, out var go)) {
                if (go != null) {
                    go.SetActive(Active);
                }
            }

            yield break;
        }
    }
}