using System;
using System.Collections;
using Shiroi.Cutscenes.Communication;
using UnityEngine.Playables;

namespace Shiroi.Cutscenes.Tokens {
    [Serializable]
    
    public class PlayableDirectorInput : Input<PlayableDirector> { }

    public class PlayTimelineToken : Token {
        public PlayableDirectorInput Director;
        public PlayableAsset PlayableAsset;

        public override IEnumerator Execute(CutsceneExecutor executor) {
            if (Director.Get(executor.Context, out var director)) {
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
}