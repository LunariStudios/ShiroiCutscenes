using System.Collections;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEngine;

namespace Shiroi.Cutscenes.Examples {
    public class MyToken : IToken {
        public Reference<GameObject> System;
        public Vector3 Position;

        public IEnumerator Execute(CutscenePlayer player) {
            var systemInScene = System.Resolve(player);
            Object.Instantiate(systemInScene, Position, Quaternion.identity);
            yield break;
        }
    }
}