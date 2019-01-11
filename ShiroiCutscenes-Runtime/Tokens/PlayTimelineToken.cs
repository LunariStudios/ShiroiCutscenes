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
            PlayableDirector director;
            if (Director.Get(executor.Context, out director)) {
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