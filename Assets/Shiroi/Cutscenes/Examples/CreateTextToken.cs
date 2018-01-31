using System.Collections;
using JetBrains.Annotations;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;
using UnityEngine.UI;

namespace Shiroi.Cutscenes.Examples {
    [UsedImplicitly]
    public class CreateTextToken : IToken, IFutureProvider, ITokenChangedListener {
        public ExposedReference<GameObject> Parent;
        public string TextName;

        [SerializeField]
        private int textId;

        public IEnumerator Execute(CutscenePlayer player) {
            var gameObject  = new GameObject(TextName);
            gameObject.transform.SetParent(Parent.Resolve(player).transform);
            var text = gameObject.AddComponent<Text>();
            text.text = TextName;
            player.ProvideFuture(text, textId);
            yield break;
        }


        public void RegisterFutures(Cutscene manager) {
            textId = manager.NotifyFuture<Text>(this, TextName);
        }

        public void OnChanged(Cutscene cutscene) {
            cutscene.FutureManager.GetFuture(textId).Name = TextName;
        }
    }
}