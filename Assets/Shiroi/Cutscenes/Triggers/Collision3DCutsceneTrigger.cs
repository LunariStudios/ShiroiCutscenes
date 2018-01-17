using UnityEngine;

namespace Shiroi.Cutscenes.Triggers {
    public class Collision3DCutsceneTrigger : CollisionCutsceneTrigger<Collider> {
        private void OnTriggerEnter(Collider other) {
            if ((Mode & CollisionMode.Enter) != CollisionMode.Enter) {
                return;
            }
            AttemptTrigger(other);
        }

        private void OnTriggerExit(Collider other) {
            if ((Mode & CollisionMode.Exit) != CollisionMode.Exit) {
                return;
            }
            AttemptTrigger(other);
        }

        private void OnTriggerStay(Collider other) {
            if ((Mode & CollisionMode.Stay) != CollisionMode.Stay) {
                return;
            }
            AttemptTrigger(other);
        }
    }
}