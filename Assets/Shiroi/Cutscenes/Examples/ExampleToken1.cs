using System.Collections;
using Shiroi.Cutscenes.Tokens;

namespace Shiroi.Cutscenes.Examples {
    public class ExampleToken1 : IToken {
        public int IntegerField;

        public IEnumerator Execute(CutscenePlayer player) {
            yield break;
        }
    }
}