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

        public FutureManager FutureManager;

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
                loadedTokens.Add(serializedToken.Deserialize());
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
            loadedTokens.Clear();
        }


        [Serializable]
        public struct SerializedToken {
            [SerializeField]
            public string TokenType;

            [SerializeField]
            public SerializedObject TokenData;

            private SerializedToken(string tokenType, SerializedObject tokenData) {
                TokenType = tokenType;
                TokenData = tokenData;
            }

            public IToken Deserialize() {
                var token = (IToken) Activator.CreateInstance(Type.GetType(TokenType));
                TokenData.Deserialize(token);
                return token;
            }

            public static SerializedToken From(IToken loadedToken) {
                var typeName = loadedToken.GetType().FullName;
                var obj = SerializedObject.From(loadedToken);
                return new SerializedToken(typeName, obj);
            }
        }
    }
}