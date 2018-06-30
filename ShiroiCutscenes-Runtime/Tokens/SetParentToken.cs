using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class SetParentToken : Token {
        public ExposedReference<GameObject> Object;
        public ExposedReference<GameObject> NewParent;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var obj = Object.Resolve(player);
            var p = NewParent.Resolve(player);
            if (!obj) {
                ShiroiCutscenesConstants.Logger.LogError("Couldn't find object to set parent");
            }
            if (!p) {
                ShiroiCutscenesConstants.Logger.LogError("Couldn't find parent object");
            }
            obj.transform.SetParent(p.transform);
            yield break;
        }
    }
}