using System.Collections;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    public class SetGameObjectActiveToken : Token {
        public ExposedReference<GameObject> Target;
        public bool Active;

        public override IEnumerator Execute(CutscenePlayer player) {
            var go = Target.Resolve(player);
            if (go != null) {
                go.SetActive(Active);
            }

            yield break;
        }
    }
}