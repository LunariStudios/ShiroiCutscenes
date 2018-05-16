using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Futures {
    [Serializable]
    public class FutureReference {
        [HideInInspector]
        public int Id;
    }

    [Serializable]
    public class FutureReference<T> : FutureReference where T : Object {
        public T Resolve(CutscenePlayer player) {
            var found = player.RequestFuture(this);
            if (found != null) {
                return found;
            }

            var typeName = typeof(T).Name;
            Debug.LogErrorFormat(
                "[ShiroiCutscenes] Future with id '{0}' ({1}) has not yet been provided!",
                Id,
                typeName);
            return null;
        }
    }
}