using System.Collections;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    /// <summary>
    /// A token represents a small action that happens inside a <see cref="Cutscene"/>.
    /// <br/>
    /// It extends from <see cref="UnityEngine.ScriptableObject"/>, so serialization behaves as specified by
    /// <a href="https://blogs.unity3d.com/pt/2012/10/25/unity-serialization/">this article</a>.
    /// </summary>
    public abstract class Token : ScriptableObject {
        /// <summary>
        /// The method called when the token is supposed to execute whatever it's action is supposed to do.
        /// This is run inside a coroutine, meanly whatever is execute will behave exactly like one.
        /// <br/>
        /// <a href="https://docs.unity3d.com/Manual/Coroutines.html">Click here for an article about coroutines</a>
        /// </summary>
        /// <param name="player">The <see cref="CutscenePlayer"/> that the cutscene is being executed in.</param>
        /// <returns>The enumerator that is executed by the <see cref="player"/> inside a coroutine.</returns>
        public abstract IEnumerator Execute(CutscenePlayer player);
    }

    /// <summary>
    /// Represents a <see cref="Token"/> that can react to changes made to itself by the inspector.
    /// </summary>
    public interface ITokenChangedListener {
        /// <summary>
        /// Called when a token is changed in the inspector
        /// </summary>
        /// <param name="cutscene"></param>
        void OnChanged(Cutscene cutscene);
    }
}