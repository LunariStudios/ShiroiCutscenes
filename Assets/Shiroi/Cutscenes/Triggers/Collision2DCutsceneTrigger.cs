using System;
using UnityEngine;

namespace Shiroi.Cutscenes.Triggers {
    public class Collision2DCutsceneTrigger : CollisionCutsceneTrigger<Collider2D> {
       

        private void OnTriggerEnter2D(Collider2D other) {
            if ((Mode & CollisionMode.Enter) != CollisionMode.Enter) {
                return;
            }
            AttemptTrigger(other);
        }

        private void OnTriggerExit2D(Collider2D other)  {
            if ((Mode & CollisionMode.Exit) != CollisionMode.Exit) {
                return;
            }
            AttemptTrigger(other);
        }

        private void OnTriggerStay2D(Collider2D other)  {
            if ((Mode & CollisionMode.Stay) != CollisionMode.Stay) {
                return;
            }
            AttemptTrigger(other);
        }
    }
}