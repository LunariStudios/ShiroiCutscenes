using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Futures {
    public class FutureTypeAttribute : Attribute {
        public FutureTypeAttribute(Type type) {
            Type = type;
        }

        public Type Type { get; private set; }
    }

    [Serializable]
    public struct FutureReference {
        public int Id;

        public T Resolve<T>(CutscenePlayer player) where T : Object {
            var found = player.Request<T>(this);
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