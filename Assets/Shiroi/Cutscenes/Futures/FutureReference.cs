using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Futures {
    [Serializable]
    public struct FutureReference<T> where T : Object {
        public int Id;

        public FutureReference(int id) {
            Id = id;
        }

        public T Resolve(CutscenePlayer player) {
            var found = player.RequestFuture(this);
            if (found != null) {
                return found;
            }
            var typeName = typeof(T).Name;
            Debug.LogErrorFormat("[ShiroiCutscenes] Future with id '{0}' ({1}) has not yet been provided!", Id,
                typeName);
            return null;
        }
    }
}