using System;
using System.Collections;
using System.Collections.Generic;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes {
    /// <summary>
    /// A collection of <see cref="Token"/>s executed in sequence in order to create a cinematic effect.
    /// </summary>
    [Serializable]
    public partial class Cutscene : ScriptableObject, IList<Token> {
        [SerializeField, HideInInspector]
        private List<Token> tokens = new List<Token>();




        public bool IsEmpty {
            get {
                return tokens.Count == 0;
            }
        }

        public void Add(int index, Token token) {
            tokens.Insert(index, token);
            CheckFutureProvider(token);
        }

        private void CheckFutureProvider(Token token) {
            var provider = token as IFutureProvider;
            if (provider != null) {
                provider.RegisterFutures(this);
            }
        }

        public void RemoveToken(int tokenIndex) {
            var token = tokens[tokenIndex];
            tokens.RemoveAt(tokenIndex);
        }

        public int IndexOf(Token item) {
            return tokens.IndexOf(item);
        }

        public void Insert(int index, Token item) {
            tokens.Insert(index, item);
        }

        public void RemoveAt(int index) {
            tokens.RemoveAt(index);
        }

        public Token this[int index] {
            get {
                return tokens[index];
            }
            set {
                tokens[index] = value;
            }
        }

        public void Add(Token item) {
            tokens.Add(item);
            CheckFutureProvider(item);
        }

        public void Clear() {
            futures.Clear();
            tokens.Clear();
        }

        public bool Contains(Token item) {
            return tokens.Contains(item);
        }

        public void CopyTo(Token[] array, int arrayIndex) {
            tokens.CopyTo(array, arrayIndex);
        }

        public bool Remove(Token item) {
            return tokens.Remove(item);
        }

        public int Count {
            get {
                return tokens.Count;
            }
        }

        public bool IsReadOnly {
            get {
                return false;
            }
        }

        public void Swap(int a, int b) {
            if (a == b) {
                return;
            }

            var element = tokens[a];
            for (var i = 0; i < tokens.Count - 1; ++i) {
                if (i >= a) {
                    tokens[i] = tokens[i + 1];
                }
            }

            for (var i = tokens.Count - 1; i > 0; --i) {
                if (i > b) {
                    tokens[i] = tokens[i - 1];
                }
            }

            tokens[b] = element;
        }

        public IEnumerator<Token> GetEnumerator() {
            return tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}