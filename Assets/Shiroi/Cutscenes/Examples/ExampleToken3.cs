using System.Collections;
using Shiroi.Cutscenes.Tokens;

namespace Shiroi.Cutscenes.Examples {
    public class ExampleToken3 : IToken {
        public IEnumerator Execute(CutscenePlayer player) {
            yield break;
        }
    }
}