using System.Collections;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    public abstract class Token : ScriptableObject {
        public abstract IEnumerator Execute(CutscenePlayer player);
    }

    public interface ITokenChangedListener {
        void OnChanged(Cutscene cutscene);
    }
}