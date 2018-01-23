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
        private List<IToken> loadedTokens = new List<IToken>();
        public static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        [SerializeField]
        private List<ExpectedFuture> futures = new List<ExpectedFuture>();

        public ExpectedFuture GetFuture(int futureId) {
            return futures.FirstOrDefault(future => future.Id == futureId);
        }

        public int NotifyFuture<T>(IFutureProvider provider, string futureName) where T : Object {
            return NotifyFuture(typeof(T), provider, futureName);
        }

        private int FindIndexOfProvider(IFutureProvider provider) {
            for (var i = 0; i < loadedTokens.Count; i++) {
                var token = loadedTokens[i];
                if (token == provider) {
                    return i;
                }
            }
            return -1;
        }

        public int NotifyFuture(Type type, IFutureProvider provider, string futureName) {
            var array = new byte[4];
            rng.GetBytes(array);
            var id = BitConverter.ToInt32(array, 0);
            var providerId = FindIndexOfProvider(provider);
            var future = new ExpectedFuture(providerId, id, type, futureName);
            futures.Add(future);
            return id;
        }

        public List<ExpectedFuture> GetFutures() {
            return new List<ExpectedFuture>(futures);
        }

        [SerializeField, HideInInspector]
        private SerializedToken[] tokens;

        public List<IToken> Tokens {
            get { return loadedTokens; }
        }

        public bool IsEmpty {
            get { return Tokens.Count == 0; }
        }

        public int TotalFutures {
            get { return futures.Count; }
        }

        public void AddToken(int index, IToken instance) {
            loadedTokens.Insert(index, instance);
            var provider = instance as IFutureProvider;
            if (provider != null) {
                provider.RegisterFutures(this);
            }
            OnReorder(index, Tokens.Count - 1);
        }

        public void AddToken(IToken token) {
            loadedTokens.Add(token);
            var provider = token as IFutureProvider;
            if (provider != null) {
                provider.RegisterFutures(this);
            }
        }

        public void RemoveToken(int tokenIndex) {
            ReorderFutures(tokenIndex);
            var token = loadedTokens[tokenIndex];
            loadedTokens.RemoveAt(tokenIndex);
        }

        private void ReorderFutures(int tokenToBeRemoved) {
            var toBeRemoved = new List<ExpectedFuture>();
            foreach (var future in futures) {
                var provider = future.Provider;
                if (provider == tokenToBeRemoved) {
                    toBeRemoved.Add(future);
                }

                //If provider is below token to be removed, nothing will change
                if (provider > tokenToBeRemoved) {
                    future.Provider--;
                }
            }
            foreach (var future in toBeRemoved) {
                futures.Remove(future);
            }
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
            futures.Clear();
            loadedTokens.Clear();
        }

        [Serializable]
        public sealed class ExpectedFuture : IComparable<ExpectedFuture>, ISerializationCallbackReceiver {
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
            private int provider;

            public int Id {
                get { return id; }
            }

            public int Provider {
                get { return provider; }
                set { provider = value; }
            }

            public Type Type { get; private set; }

            public string Name {
                get { return name; }
                set { name = value; }
            }

            public ExpectedFuture(int provider, int id, Type type, string name) {
                this.provider = provider;
                this.id = id;
                Type = type;
                this.name = name;
            }

            public void OnBeforeSerialize() {
                type = Type.AssemblyQualifiedName;
            }

            public void OnAfterDeserialize() {
                Type = Type.GetType(type);
            }

            public int CompareTo(ExpectedFuture other) {
                return provider.CompareTo(other.provider);
            }
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

        public int IndexOfFuture(int futureId) {
            for (var i = 0; i < futures.Count; i++) {
                var expectedFuture = futures[i];
                if (expectedFuture.Id == futureId) {
                    return i;
                }
            }
            return -1;
        }

        public void OnReorder(int newIndex, int oldIndex) {
            var toModify = Math.Sign(oldIndex - newIndex);
            var min = Mathf.Min(newIndex, oldIndex);
            var max = Mathf.Max(newIndex, oldIndex);
            foreach (var future in futures) {
                //Check if pointed to old
                var provider = future.Provider;
                if (provider == oldIndex) {
                    future.Provider = newIndex;
                    continue;
                }
                if (provider >= min && provider <= max) {
                    future.Provider += toModify;
                }
            }
        }

        public IEnumerable<ExpectedFuture> GetFuturesOf(int providerIndex) {
            return from future in futures where future.Provider == providerIndex select future;
        }
    }
}