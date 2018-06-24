using System.Collections;

namespace Shiroi.Cutscenes.Tokens {
    public class SetFloatToken : Token {
        public string Key;
        public float Value;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            executor.SetMeta(Key, Value);
            yield break;
        }
    }
}