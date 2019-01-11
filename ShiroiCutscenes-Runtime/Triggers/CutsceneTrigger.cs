using System;
using UnityEngine;

namespace Shiroi.Cutscenes.Triggers {
    public abstract class CutsceneTrigger : MonoBehaviour {
        public enum PostPlayAction {
            None,
            Disable,
            DestroyGameObject,
            DestroySelf
        }

        public Cutscene Cutscene;
        public PostPlayAction PostAction = PostPlayAction.DestroyGameObject;
        private bool played;

        protected void Trigger() {
            if (PostAction != PostPlayAction.None && played) {
                return;
            }

            played = true;
            //Player.Play(Cutscene);
            switch (PostAction) {
                case PostPlayAction.None:
                    break;
                case PostPlayAction.Disable:
                    enabled = false;
                    break;
                case PostPlayAction.DestroyGameObject:
                    Destroy(gameObject);
                    break;
                case PostPlayAction.DestroySelf:
                    Destroy(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}