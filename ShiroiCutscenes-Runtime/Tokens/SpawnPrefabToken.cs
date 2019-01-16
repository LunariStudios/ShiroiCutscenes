using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.ComponentModel;
using Shiroi.Cutscenes.Communication;
using Shiroi.Cutscenes.Preview;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    [Category(ShiroiCutscenesConstants.CommonCategory)]
    public class SpawnPrefabToken : Token, IScenePreviewable, IOutputContext {
        public GameObject Obj;
        public GameObjectOutput Output;
        public Vector3 Position;
        public Quaternion Rotation;

        public override IEnumerator Execute(CutsceneExecutor executor) {
            var obj = Instantiate(Obj, Position, Rotation);
            Output.Apply(obj, executor.Context);
            yield break;
        }

        public void OnPreview(ISceneHandle handle) {
            handle.Label(Position, string.Format("Prefab Spawn Position ({0})", Output.Name));
            Position = handle.PositionHandle(Position);
            Rotation = handle.RotationHandle(Position, Rotation);
        }

        public IEnumerable<Output> GetOutputs() {
            yield return Output;
        }
    }
}