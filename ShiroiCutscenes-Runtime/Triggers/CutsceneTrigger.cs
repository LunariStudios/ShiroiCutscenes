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
        public CutscenePlayer Player;
        public PostPlayAction PostAction = PostPlayAction.DestroyGameObject;
        private bool played;

        protected void Trigger() {
            if (!Player) {
                Debug.LogWarning("[ShiroiCutscenes] Couldn't find an active instance of CutscenePlayer!");
                return;
            }

            if (PostAction != PostPlayAction.None && played) {
                return;
            }

            played = true;
            Player.Play(Cutscene);
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