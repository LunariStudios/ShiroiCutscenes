using System.Collections;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes.Examples {
    public class ExampleToken3 : IToken {
        public ExposedReference<Camera> Camera;
        public ExposedReference<Collider> Collider;
        public Bounds Bounds;
        public BoundsInt BoundsInt;
        public IEnumerator Execute(CutscenePlayer player) {
            yield break;
        }
    }
}