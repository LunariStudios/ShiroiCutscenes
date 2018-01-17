using System.Collections;
using Shiroi.Cutscenes.Tokens;

namespace Shiroi.Cutscenes.Examples {
    public class ExampleToken2 : IToken {
        public float FloatField;

        public IEnumerator Execute(CutscenePlayer player) {
            yield break;
        }
    }
}