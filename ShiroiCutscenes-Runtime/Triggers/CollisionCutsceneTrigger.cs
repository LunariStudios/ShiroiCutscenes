using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shiroi.Cutscenes.Triggers {
    public abstract class CollisionFilter<C> : MonoBehaviour {
        public abstract bool ShouldDisallow(C collider);
    }

    public sealed class CollisionFilterHandler<C> {
        private List<CollisionFilter<C>> filters = new List<CollisionFilter<C>>();

        public bool ShouldDisallow(C collider) {
            return filters.Any(filter => filter.ShouldDisallow(collider));
        }

        public void LoadFrom(GameObject obj) {
            filters.AddRange(obj.GetComponentsInChildren<CollisionFilter<C>>());
        }
    }
    public abstract class CollisionCutsceneTrigger<C> : CutsceneTrigger {
        [Flags]
        public enum CollisionMode {
            Enter = 1 << 0,
            Exit = 1 << 1,
            Stay = 1 << 2
        }

        public CollisionMode Mode;
        private CollisionFilterHandler<C> handler = new CollisionFilterHandler<C>();
        private void Awake() {
            handler.LoadFrom(gameObject);
        }

        protected void AttemptTrigger(C collider) {
            if (handler.ShouldDisallow(collider)) {
                return;
            }
            Trigger();
        }
    }
}