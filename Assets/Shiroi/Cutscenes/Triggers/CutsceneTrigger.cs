using UnityEngine;

namespace Shiroi.Cutscenes.Triggers {
    public abstract class CutsceneTrigger : MonoBehaviour {
        public Cutscene Cutscene;

        protected void Trigger() {
            var player = CutscenePlayer.Instance;
            if (!player) {
                Debug.LogWarning("[ShiroiCutscenes] Couldn't find an active instance of CutscenePlayer!");
                return;
            }
            player.Play(Cutscene);
        }
    }
}