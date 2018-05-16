using System;
using System.Collections.Generic;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes {
    [Serializable]
    public class Cutscene : ScriptableObject {
        [SerializeField, HideInInspector]
        private List<Token> tokens = new List<Token>();

        public FutureManager FutureManager = new FutureManager();

        public int NotifyFuture<T>(IFutureProvider provider, string futureName) where T : Object {
            return FutureManager.NotifyFuture<T>(this, provider, futureName);
        }

        public List<Token> Tokens {
            get {
                return tokens;
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

        public void AddToken(int index, Token instance) {
            FutureManager.OnReorder(index, Tokens.Count);
            tokens.Insert(index, instance);
            var provider = instance as IFutureProvider;
            if (provider != null) {
                provider.RegisterFutures(this);
            }
        }

        public void AddToken(Token token) {
            tokens.Add(token);
            var provider = token as IFutureProvider;
            if (provider != null) {
                provider.RegisterFutures(this);
            }
        }

        public void RemoveToken(int tokenIndex) {
            FutureManager.ReorderFutures(tokenIndex);
            var token = tokens[tokenIndex];
            tokens.RemoveAt(tokenIndex);
        }

        public Token this[int index] {
            get {
                return tokens[index];
            }
            set {
                tokens[index] = value;
            }
        }

        public void Clear() {
            FutureManager.Clear();
            tokens.Clear();
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
            FutureManager.OnReorder(b, a);
        }
    }
}