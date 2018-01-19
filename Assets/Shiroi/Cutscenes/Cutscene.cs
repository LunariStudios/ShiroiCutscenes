using System;
using System.Collections.Generic;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes {
    [CreateAssetMenu(menuName = CreateCutsceneMenuPath), Serializable]
    public class Cutscene : ScriptableObject, ISerializationCallbackReceiver {
        public const string CreateCutsceneMenuPath = "Shiroi/Cutscenes/Cutscene";
        private List<IToken> loadedTokens = new List<IToken>();

        [SerializeField]
        private List<ExpectedFuture> futures = new List<ExpectedFuture>();

        public int NotifyFuture<T>(IFutureProvider provider) where T : Object {
            return NotifyFuture(typeof(T), provider);
        }

        private uint FindIndexOfProvider(IFutureProvider provider) {
            for (var i = 0; i < loadedTokens.Count; i++) {
                var token = loadedTokens[i];
                if (token == provider) {
                    return (uint) i;
                }
            }
            return uint.MaxValue;
        }

        public int NotifyFuture(Type type, IFutureProvider provider) {
            var id = futures.Count;
            var providerId = FindIndexOfProvider(provider);
            var future = new ExpectedFuture(providerId, id, type);
            futures.Add(future);
            return id;
        }

        public ExpectedFuture[] GetFutures() {
            return futures.ToArray();
        }

        [SerializeField, HideInInspector]
        private SerializedToken[] tokens;

        public List<IToken> Tokens {
            get { return loadedTokens; }
        }

        public bool IsEmpty {
            get { return Tokens.Count == 0; }
        }

        public void AddToken(IToken token) {
            loadedTokens.Add(token);
            var provider = token as IFutureProvider;
            if (provider != null) {
                provider.RegisterFutures(this);
            }
        }

        public void RemoveToken(int tokenIndex) {
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
            get { return loadedTokens[index]; }
            set { loadedTokens[index] = value; }
        }

        public void Clear() {
            loadedTokens.Clear();
        }

        [Serializable]
        public sealed class ExpectedFuture : ISerializationCallbackReceiver {
            [SerializeField]
            private string type;

            [SerializeField]
            private string name;

            /**
             * The id of this future, this is the number that will be used when trying to find a future. 
             */
            [SerializeField]
            private int id;

            /**
             * The provider of this future, this is the index of the provider that is responsible for providing
             * this future
             */
            [SerializeField]
            private uint provider;

            public int Id {
                get { return id; }
            }

            public uint Provider {
                get { return provider; }
            }

            public Type Type { get; private set; }

            public string Name {
                get { return name; }
            }

            public ExpectedFuture(uint provider, int id, Type type) {
                this.provider = provider;
                this.id = id;
                Type = type;
            }

            public void OnBeforeSerialize() {
                type = Type.AssemblyQualifiedName;
            }

            public void OnAfterDeserialize() {
                Type = Type.GetType(type);
            }
        }

        [Serializable]
        public struct SerializedToken {
            [SerializeField]
            public string TokenType;

            [SerializeField]
            public byte[] TokenData;

            private SerializedToken(string tokenType, byte[] tokenData) {
                TokenType = tokenType;
                TokenData = tokenData;
            }

            public IToken Deserialize() {
                return (IToken) JsonUtility.FromJson(Base64Util.Base64Decode(TokenData), Type.GetType(TokenType));
            }

            public static SerializedToken From(IToken loadedToken) {
                var typeName = loadedToken.GetType().AssemblyQualifiedName;
                var json = Base64Util.Base64Encode(JsonUtility.ToJson(loadedToken));
                return new SerializedToken(typeName, json);
            }
        }

        public uint IndexOfFuture(int futureId) {
            return 0;
        }

        public string[] GetFutureNames() {
            throw new NotImplementedException();
        }
    }
}