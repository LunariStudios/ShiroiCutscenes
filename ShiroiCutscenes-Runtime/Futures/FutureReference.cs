using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Futures {
    public class FutureReference {
        public int Id;

        public FutureReference(int id) {
            Id = id;
        }
    }

    [Serializable]
    public sealed class FutureReference<T> : FutureReference where T : Object {
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

        public FutureReference(int id) : base(id) { }
    }
}