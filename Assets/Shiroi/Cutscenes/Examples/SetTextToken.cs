using System.Collections;
using JetBrains.Annotations;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;
using UnityEngine.UI;

namespace Shiroi.Cutscenes.Examples {
    [UsedImplicitly]
    public class SetFutureTextToken : IToken {
        [FutureType(typeof(Text))]
        public FutureReference Text;

        public Font Font;
        public string Content = "Hello from ShiroiCutscenes!";
        public Vector2 Position;
        public Vector2 Size;

        public IEnumerator Execute(CutscenePlayer player) {
            var t = Text.Resolve<Text>(player);
            t.text = Content;
            t.font = Font;
            var rect = t.rectTransform;
            rect.position = Position;
            rect.sizeDelta = Size;
            yield break;
        }
    }
}