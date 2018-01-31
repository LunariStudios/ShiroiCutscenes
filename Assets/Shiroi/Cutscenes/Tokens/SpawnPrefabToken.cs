using System.Collections;
using JetBrains.Annotations;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Preview;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class SpawnPrefabToken : IToken, IFutureProvider, ITokenChangedListener, IScenePreviewable {
        public GameObject Obj;
        public string FutureName = "future_name";
        public Vector3 Position;
        public Quaternion Rotation;

        public IEnumerator Execute(CutscenePlayer player) {
            var obj = Object.Instantiate(Obj, Position, Rotation);
            player.ProvideFuture(obj, futureId);
            yield break;
        }

        [SerializeField]
        private int futureId;

        public void RegisterFutures(Cutscene manager) {
            futureId = manager.NotifyFuture<GameObject>(this, FutureName);
        }

        public void OnChanged(Cutscene cutscene) {
            cutscene.FutureManager.GetFuture(futureId).Name = FutureName;
        }

        public void OnPreview(ISceneHandle handle, SceneView sceneView) {
            handle.Label(Position, string.Format("Prefab Spawn Position ({0})", FutureName));
            Position = handle.PositionHandle(Position);
            Rotation = handle.RotationHandle(Position, Rotation);
        }
    }
}