using System.Collections;
using JetBrains.Annotations;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;
using UnityEngine.UI;

namespace Shiroi.Cutscenes.Examples {
    [UsedImplicitly]
    public class SetExposedTextToken : IToken {
        public ExposedReference<Text> Text;
        public Font Font;
        public string Content = "Hello from ShiroiCutscenes!";
        public Vector2 Position;
        public Vector2 Size;

        public IEnumerator Execute(CutscenePlayer player) {
            var t = Text.Resolve(player);
            t.text = Content;

            var rect = t.rectTransform;
            rect.position = Position;
            rect.sizeDelta = Size;
            yield break;
        }
    }
}