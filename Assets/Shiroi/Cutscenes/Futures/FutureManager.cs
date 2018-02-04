using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Futures {
    [Serializable]
    public sealed class FutureManager {
        private static readonly RandomNumberGenerator FutureIDGenerator = RandomNumberGenerator.Create();
        public const string DefaultFutureName = "unnamed_future";

        public int TotalFutures {
            get {
                return futures.Count;
            }
        }

        public IEnumerable<ExpectedFuture> Futures {
            get {
                return futures;
            }
        }

        [SerializeField]
        private List<ExpectedFuture> futures = new List<ExpectedFuture>();

        public ExpectedFuture GetFuture(int futureId) {
            return futures.FirstOrDefault(future => future.Id == futureId);
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

        private static int FindIndexOfProvider(IFutureProvider provider, List<IToken> loadedTokens) {
            for (var i = 0; i < loadedTokens.Count; i++) {
                var token = loadedTokens[i];
                if (token == provider) {
                    return i;
                }
            }
            return -1;
        }

        public int NotifyFuture<T>(Cutscene cutscene, IFutureProvider provider, string futureName) where T : Object {
            return NotifyFuture(cutscene, typeof(T), provider, futureName);
        }

        public int NotifyFuture(Cutscene cutscene, Type type, IFutureProvider provider, string futureName) {
            if (string.IsNullOrEmpty(futureName)) {
                futureName = DefaultFutureName;
            }
            var array = new byte[sizeof(int)];
            FutureIDGenerator.GetBytes(array);
            var id = BitConverter.ToInt32(array, 0);
            var providerId = FindIndexOfProvider(provider, cutscene.Tokens);
            var future = new ExpectedFuture(providerId, id, type, futureName);
            futures.Add(future);
            return id;
        }

        public List<ExpectedFuture> GetFutures() {
            return new List<ExpectedFuture>(futures);
        }

        public void Clear() {
            futures.Clear();
        }

        public void ReorderFutures(int tokenToBeRemoved) {
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
            get {
                return id;
            }
        }

        public int Provider {
            get {
                return provider;
            }
            set {
                provider = value;
            }
        }

        public Type Type {
            get;
            private set;
        }

        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
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

        public override string ToString() {
            return string.Format("ExpectedFuture(Type: {0}, Name: {1}, ID: {2}, Provider: {3})", type, name, id,
                provider);
        }
    }
}