using System.Collections;
using Shiroi.Cutscenes.Attributes;
using Shiroi.Cutscenes.Communication;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [TokenCategory(ShiroiCutscenesConstants.CommonCategory)]
    public class SetGameObjectActiveToken : Token {
        public GameObjectInput Target;
        public bool Active;

        public override IEnumerator Execute(CutsceneExecutor executor) {
            GameObject go;
            if (Target.Get(executor.Context, out go)) {
                if (go != null) {
                    go.SetActive(Active);
                }
            }

            yield break;
        }
    }
}