using System.Collections;

namespace Shiroi.Cutscenes.Tokens {
    public interface IToken {
        IEnumerator Execute(CutscenePlayer player);
    }
}