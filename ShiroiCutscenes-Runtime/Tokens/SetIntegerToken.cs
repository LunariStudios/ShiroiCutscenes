using System.Collections;

namespace Shiroi.Cutscenes.Tokens {
    public class SetIntegerToken : Token {
        public string Key;
        public int Value;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            executor.SetMeta(Key, Value);
            yield break;
        }
    }
}