using System.Collections;

namespace Shiroi.Cutscenes.Tokens {
    public class SetStringToken : Token {
        public string Key;
        public string Value;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            executor.SetMeta(Key, Value);
            yield break;
        }
    }
}