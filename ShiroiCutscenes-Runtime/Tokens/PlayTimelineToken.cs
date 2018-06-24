using System;
using System.Collections;
using Shiroi.Cutscenes.Util;
using UnityEngine;
using UnityEngine.Playables;

namespace Shiroi.Cutscenes.Tokens {
    public class PlayTimelineToken : Token {
        public ExposedReference<PlayableDirector> Director;
        public PlayableAsset PlayableAsset;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var director = Director.Resolve(player);
            if (director.playableAsset != PlayableAsset) {
                director.playableAsset = PlayableAsset;
            }

            director.Play();
            while (director.state == PlayState.Playing) {
                yield return null;
            }
        }
    }
}