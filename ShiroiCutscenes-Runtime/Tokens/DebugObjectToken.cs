using System.Collections;
using System.ComponentModel;
using Shiroi.Cutscenes.Communication;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [Category(ShiroiCutscenesConstants.DebugCategory)]
    public class DebugObjectToken : Token {
        public ObjectInput Object;

        public override IEnumerator Execute(CutsceneExecutor executor) {
            if (Object.Get(executor.Context, out var result)) {
                var msg = result == null ? $"No object found at token {name} using input {Object.Name}" : $"Found object: '{result}'";
                Debug.Log(msg);
            } else {
                Debug.Log($"Unable to retrieve object from input {Object.Name}");
            }
            yield break;
        }
    }
}