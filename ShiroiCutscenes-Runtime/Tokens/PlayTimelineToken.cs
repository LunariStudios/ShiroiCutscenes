using System;
using System.Collections;
using Shiroi.Cutscenes.Util;
using UnityEngine.Playables;

namespace Shiroi.Cutscenes.Tokens {
    public class PlayTimelineToken : Token {
        public DirectorReference Director;
        public PlayableAsset PlayableAsset;
        private bool playing;

        public override IEnumerator Execute(CutscenePlayer player) {
            var director = Director.Resolve(player);
            if (director.playableAsset != PlayableAsset) {
                director.playableAsset = PlayableAsset;
            }

            director.Play();
            Action<PlayableDirector> listener = null;
            listener = delegate(PlayableDirector playableDirector) {
                OnPlayed(playableDirector);
                director.stopped -= listener;
            };
            director.stopped += listener;
            playing = true;
            while (playing) {
                yield return null;
            }
        }

        private void OnPlayed(PlayableDirector obj) {
            playing = false;
        }
    }
}