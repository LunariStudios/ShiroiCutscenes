using System.Collections;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    public class SpawnGameObjectToken : IToken {
        public GameObject Obj;
        public Vector3 Position;
        public Quaternion Rotation;

        public IEnumerator Execute(CutscenePlayer player) {
            Object.Instantiate(Obj, Position, Rotation);
            yield break;
        }
    }
}