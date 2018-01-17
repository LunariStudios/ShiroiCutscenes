using System;
using System.Collections.Generic;
using System.Linq;
using Shiroi.Cutscenes.Serialization;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes {
    [Serializable]
    public class CutsceneLine : ISerializationCallbackReceiver {
        public float Delay;
        private List<IToken> tokens = new List<IToken>();

        public List<IToken> Tokens {
            get { return tokens; }
        }

        [SerializeField, HideInInspector]
        private List<SerializedComplex> serializedTokens = new List<SerializedComplex>();

        public void OnBeforeSerialize() {
            foreach (var token in tokens) {
                var complex = SerializedComplex.FromToken(token);
                serializedTokens.Add(complex);
            }
            //tokens.Clear();
        }

        public void OnAfterDeserialize() {
            tokens = new List<IToken>();
            foreach (var complex in serializedTokens) { }
            serializedTokens.Clear();
        }

        public IToken this[int index] {
            get { return tokens[index]; }
            set { tokens[index] = value; }
        }

        public void AddToken(IToken token) {
            tokens.Add(token);
        }

        public void RemoveToken(int reorderableListIndex) {
            tokens.RemoveAt(reorderableListIndex);
        }
    }
}