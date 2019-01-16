using System;
using System.Collections;
using System.Collections.Generic;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes {
    /// <summary>
    /// A collection of <see cref="Token"/>s executed in sequence in order to create a cinematic effect.
    /// </summary>
    [Serializable]
    public partial class Cutscene : ScriptableObject, IList<Token> {
        [SerializeField, HideInInspector]
        private List<Token> tokens = new List<Token>();

        public IList<Token> Tokens => tokens;

        public bool IsEmpty => tokens.Count == 0;

        public void Add(int index, Token token) {
            tokens.Insert(index, token);
        }


        public void RemoveToken(int tokenIndex) {
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
            get => tokens[index];
            set => tokens[index] = value;
        }

        public void Add(Token item) {
            tokens.Add(item);
        }

        public void Clear() {
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

        public int Count => tokens.Count;

        public bool IsReadOnly => false;

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

        public Token GetToken(int i) {
            return tokens[i];
        }

    }
}