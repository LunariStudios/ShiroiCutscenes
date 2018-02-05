using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Serialization;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes {
    [CreateAssetMenu(menuName = CreateCutsceneMenuPath), Serializable]
    public class Cutscene : ScriptableObject, ISerializationCallbackReceiver {
        public const string CreateCutsceneMenuPath = "Shiroi/Cutscenes/Cutscene";
        private readonly List<IToken> loadedTokens = new List<IToken>();


        [SerializeField, HideInInspector]
        private SerializedToken[] tokens;

        public FutureManager FutureManager = new FutureManager();

        public int NotifyFuture<T>(IFutureProvider provider, string futureName) where T : Object {
            return FutureManager.NotifyFuture<T>(this, provider, futureName);
        }

        public List<IToken> Tokens {
            get {
                return loadedTokens;
            }
        }

        public bool IsEmpty {
            get {
                return Tokens.Count == 0;
            }
        }

        public int TotalTokens {
            get {
                return Tokens.Count;
            }
        }

        public void AddToken(int index, IToken instance) {
            FutureManager.OnReorder(index, Tokens.Count);
            loadedTokens.Insert(index, instance);
            var provider = instance as IFutureProvider;
            if (provider != null) {
                provider.RegisterFutures(this);
            }
        }

        public void AddToken(IToken token) {
            loadedTokens.Add(token);
            var provider = token as IFutureProvider;
            if (provider != null) {
                provider.RegisterFutures(this);
            }
        }

        public void RemoveToken(int tokenIndex) {
            FutureManager.ReorderFutures(tokenIndex);
            var token = loadedTokens[tokenIndex];
            loadedTokens.RemoveAt(tokenIndex);
        }


        public void OnBeforeSerialize() {
            tokens = new SerializedToken[loadedTokens.Count];
            for (var i = 0; i < loadedTokens.Count; i++) {
                tokens[i] = SerializedToken.From(loadedTokens[i]);
            }
        }

        public void OnAfterDeserialize() {
            if (tokens == null) {
                return;
            }
            foreach (var serializedToken in tokens) {
                var deserialized = serializedToken.Deserialize();
                if (deserialized == null) {
                    continue;
                }
                loadedTokens.Add(deserialized);
            }
            tokens = null;
        }

        public IToken this[int index] {
            get {
                return loadedTokens[index];
            }
            set {
                loadedTokens[index] = value;
            }
        }

        public void Clear() {
            FutureManager.Clear();
            loadedTokens.Clear();
        }

        public void Swap(int a, int b) {
            if (a == b) {
                return;
            }
            var element = loadedTokens[a];
            for (var i = 0; i < loadedTokens.Count - 1; ++i) {
                if (i >= a) {
                    loadedTokens[i] = loadedTokens[i + 1];
                }
            }
            for (var i = loadedTokens.Count - 1; i > 0; --i) {
                if (i > b) {
                    loadedTokens[i] = loadedTokens[i - 1];
                }
            }
            loadedTokens[b] = element;
            FutureManager.OnReorder(b, a);
        }
    }
}