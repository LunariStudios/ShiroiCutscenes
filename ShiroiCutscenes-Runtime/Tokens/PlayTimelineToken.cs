using System;
using System.Collections;
using Shiroi.Cutscenes.Util;
using UnityEngine;
using UnityEngine.Playables;

namespace Shiroi.Cutscenes.Tokens {
    public class PlayTimelineToken : Token {
        public ExposedReference<PlayableDirector> Director;
        public PlayableAsset PlayableAsset;
        private bool playing;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var director = Director.Resolve(player);
            if (director.playableAsset != PlayableAsset) {
                director.playableAsset = PlayableAsset;
            }

            director.Play();
            yield return new WaitForSeconds((float) PlayableAsset.duration);
        }

        private void OnPlayed(PlayableDirector obj) {
            playing = false;
        }
    }
}