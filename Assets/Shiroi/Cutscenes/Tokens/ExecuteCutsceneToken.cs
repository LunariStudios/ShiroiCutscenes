using System.Collections;

namespace Shiroi.Cutscenes.Tokens {
    public class ExecuteCutsceneToken : IToken {
        public Cutscene Cutscene;

        public IEnumerator Execute(CutscenePlayer player) {
            yield break;
        }
    }
}