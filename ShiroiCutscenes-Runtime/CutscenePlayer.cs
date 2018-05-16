using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shiroi.Cutscenes.Futures;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes {
    public class CutscenePlayer : MonoBehaviour, IExposedPropertyTable {
        //Futures
        private Dictionary<int, Object> providedFutures = new Dictionary<int, Object>();


        public void ProvideFuture<T>(T future, int id) where T : Object {
            providedFutures[id] = future;
        }

        public T RequestFuture<T>(FutureReference<T> reference) where T : Object {
            Object future;
            if (TryGetFuture(reference, out future)) {
                return (T) future;
            }
            return null;
        }

        public T RequestFuture<T>(int reference) where T : Object {
            Object future;
            if (TryGetFuture(reference, out future)) {
                return (T) future;
            }
            return null;
        }
        public Object RequestFuture(int reference) {
            Object future;
            if (TryGetFuture(reference, out future)) {
                return future;
            }
            return null;
        }

        private bool TryGetFuture<T>(FutureReference<T> reference, out Object future) where T : Object {
            var id = reference.Id;
            if (providedFutures.ContainsKey(id)) {
                future = providedFutures[id];
                return true;
            }
            future = null;
            return false;
        }

        private bool TryGetFuture(int id, out Object future) {
            if (providedFutures.ContainsKey(id)) {
                future = providedFutures[id];
                return true;
            }
            future = null;
            return false;
        }

        public void Play(Cutscene cutscene) {
            StartCoroutine(YieldPlay(cutscene));
        }

        public IEnumerator YieldPlay(Cutscene cutscene) {
            foreach (var token in cutscene.Tokens) {
                yield return token.Execute(this);
            }
        }

        [SerializeField, HideInInspector]
        private List<SceneReference> references = new List<SceneReference>();

        public List<SceneReference> References {
            get { return references; }
        }

        public Object GetReferenceValue(PropertyName id, out bool idValid) {
            var found = FindReference(id);
            var has = found != null;
            idValid = has;
            return has ? found.Object : null;
        }

        public void SetReferenceValue(PropertyName id, Object value) {
            var reference = FindReference(id);
            if (reference == null) {
                if (value == null) {
                    return;
                }
                reference = new SceneReference(id.GetHashCode(), value);
                references.Add(reference);
            }
#if UNITY_EDITOR
            if (value == null) {
                reference.TotalUses--;
                if (reference.TotalUses <= 0) {
                    ClearReferenceValue(id);
                }
                return;
            }
            reference.TotalUses++;
#endif
            reference.Object = value;
        }

        private SceneReference FindReference(PropertyName id) {
            return references.FirstOrDefault(refe => refe.Id == id.GetHashCode());
        }

        public void ClearReferenceValue(PropertyName id) {
            var reference = FindReference(id);
            if (!references.Contains(reference)) {
                return;
            }
            references.Remove(reference);
        }

        [Serializable]
        public sealed class SceneReference : IComparable<SceneReference> {
#if UNITY_EDITOR
            public int TotalUses = 0;
#endif

            [SerializeField]
            public int Id;

            [SerializeField]
            public Object Object;

            public SceneReference(int id, Object o) {
                Id = id;
                Object = o;
            }

            private SceneReference() { }

            public int CompareTo(SceneReference other) {
                return Id.CompareTo(other.Id);
            }
        }

        public void ClearReferences() {
            References.Clear();
        }
    }
}