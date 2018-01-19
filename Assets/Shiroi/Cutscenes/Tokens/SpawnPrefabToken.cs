using System.Collections;
using JetBrains.Annotations;
using Shiroi.Cutscenes.Futures;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class SpawnPrefabToken : IToken, IFutureProvider {
        public GameObject Obj;
        public Vector3 Position;
        public Quaternion Rotation;

        public IEnumerator Execute(CutscenePlayer player) {
            var obj = Object.Instantiate(Obj, Position, Rotation);
            yield break;
        }

        [SerializeField]
        private int gameObjectID;

        public void RegisterFutures(Cutscene manager) {
            if (Obj == null) {
                return;
            }
            gameObjectID = manager.NotifyFuture<GameObject>(this);
            foreach (var component in Obj.GetComponents(typeof(Component))) {
                manager.NotifyFuture(component.GetType(), this);
            }
        }
    }
}