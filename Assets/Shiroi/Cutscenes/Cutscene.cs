using System;
using System.Collections.Generic;
using System.Linq;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEngine;

namespace Shiroi.Cutscenes {
    [CreateAssetMenu(menuName = CreateCutsceneMenuPath), Serializable]
    public class Cutscene : ScriptableObject, ISerializationCallbackReceiver {
        public const string CreateCutsceneMenuPath = "Shiroi/Cutscenes/Cutscene";
        private List<IToken> loadedTokens = new List<IToken>();

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
        }

        public void RemoveToken(int tokenIndex) {
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
    }
}