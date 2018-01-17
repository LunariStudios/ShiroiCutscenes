using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class SetParentToken : IToken {
        public ExposedReference<GameObject> Object;
        public ExposedReference<GameObject> NewParent;

        public IEnumerator Execute(CutscenePlayer player) {
            var obj = Object.Resolve(player);
            var p = NewParent.Resolve(player);
            if (!obj) {
                Debug.LogError("Couldn't find object to set parent");
            }
            if (!p) {
                Debug.LogError("Couldn't find parent object");
            }
            obj.transform.SetParent(p.transform);
            yield break;
        }
    }
}